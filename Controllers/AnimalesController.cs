using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRancho.API.Data;
using SmartRancho.API.Entities;
using SmartRancho.API.Entities.Enums;
using SmartRancho.API.Dtos;

namespace SmartRancho.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnimalesController : ControllerBase
{
    private readonly SmartRanchoDbContext _context;

    public AnimalesController(SmartRanchoDbContext context)
    {
        _context = context;
    }

    private int GetRanchoId()
    {
        var claim = User.FindFirst("RanchoId")?.Value;

        if (string.IsNullOrEmpty(claim))
            throw new Exception("Token inválido");

        return int.Parse(claim);
    }

    [HttpPost]
    public async Task<IActionResult> CrearAnimal(AnimalCreateDto dto)
    {
        var ranchoId = GetRanchoId();

        var animal = new Animal
        {
            RanchoId = ranchoId,
            NumeroArete = dto.NumeroArete,
            Nombre = dto.Nombre,
            Sexo = dto.Sexo,
            Raza = dto.Raza,
            FechaNacimiento = dto.FechaNacimiento,
            Color = dto.Color,
            Observaciones = dto.Observaciones,
            PotreroId = dto.PotreroId,
            Estado = EstadoAnimal.Activo,
            FechaRegistro = DateTime.UtcNow
        };

        _context.Animales.Add(animal);
        await _context.SaveChangesAsync();

        return Ok(animal);
    }

    [HttpGet]
    public IActionResult GetAnimales()
    {
        var ranchoId = GetRanchoId();

        var animales = _context.Animales
            .AsNoTracking()
            .Where(a => a.RanchoId == ranchoId)
            .Select(a => new
            {
                a.AnimalId,
                a.NumeroArete,
                a.Nombre,
                a.Sexo,
                a.Raza,
                a.FechaNacimiento,
                a.Color,
                a.Observaciones,
                a.Estado,
                a.PotreroId,

                potreroNombre = a.Potrero != null
                    ? a.Potrero.Nombre
                    : null
            })
            .ToList();

        return Ok(animales);
    }

    [HttpGet("buscar/{arete}")]
    public IActionResult BuscarPorArete(string arete)
    {
        var ranchoId = GetRanchoId();

        var animal = _context.Animales
            .AsNoTracking()
            .FirstOrDefault(a => a.RanchoId == ranchoId && a.NumeroArete == arete);

        if (animal == null)
            return NotFound("Animal no encontrado");

        return Ok(animal);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditarAnimal(int id, AnimalCreateDto dto)
    {
        var ranchoId = GetRanchoId();

        var animal = _context.Animales
            .FirstOrDefault(a => a.AnimalId == id && a.RanchoId == ranchoId);

        if (animal == null)
            return NotFound("Animal no encontrado");

        animal.NumeroArete = dto.NumeroArete;
        animal.Nombre = dto.Nombre;
        animal.Sexo = dto.Sexo;
        animal.Raza = dto.Raza;
        animal.FechaNacimiento = dto.FechaNacimiento;
        animal.Color = dto.Color;
        animal.Observaciones = dto.Observaciones;
        animal.PotreroId = dto.PotreroId;

        await _context.SaveChangesAsync();

        return Ok(animal);
    }

    [HttpPatch("{id}/estado")]
    public async Task<IActionResult> CambiarEstadoAnimal(int id, [FromBody] EstadoAnimal nuevoEstado)
    {

        var ranchoId = GetRanchoId();

        var animal = _context.Animales
            .FirstOrDefault(a => a.AnimalId == id && a.RanchoId == ranchoId);

        if (animal == null)
            return NotFound("Animal no encontrado");

        var estadoAnterior = animal.Estado;

        if (!EsTransicionValida(estadoAnterior, nuevoEstado))
            return BadRequest($"No se puede cambiar de {estadoAnterior} a {nuevoEstado}");

        animal.Estado = nuevoEstado;

        _context.AnimalEstadoHistorial.Add(new AnimalEstadoHistorial
        {
            AnimalId = animal.AnimalId,
            Estado = nuevoEstado,
            FechaEstado = DateTime.UtcNow,
            Motivo = $"Cambio de {estadoAnterior} a {nuevoEstado}"
        });

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Estado actualizado correctamente",
            estadoAnterior,
            nuevoEstado
        });
    }
    
    [HttpGet("{id}/historial")]
    public IActionResult GetHistorialAnimal(int id)
    {
        var ranchoId = GetRanchoId();

        var animal = _context.Animales
            .FirstOrDefault(a => a.AnimalId == id && a.RanchoId == ranchoId);

        if (animal == null)
            return NotFound("Animal no encontrado");

        var historial = _context.AnimalEstadoHistorial
            .Where(h => h.AnimalId == id)
            .OrderByDescending(h => h.FechaEstado)
            .Select(h => new
            {
                h.Estado,
                h.FechaEstado,
                h.Motivo
            })
            .ToList();

        return Ok(new
        {
            animal.AnimalId,
            animal.NumeroArete,
            historial
        });
    }
    private bool EsTransicionValida(EstadoAnimal actual, EstadoAnimal nuevo)
    {
        if (actual == EstadoAnimal.Muerto)
            return false;

        if (actual == EstadoAnimal.Vendido && nuevo == EstadoAnimal.Activo)
            return false;

        return true;
    }

    [HttpGet("estado/{estado}")]
    public IActionResult GetAnimalesPorEstado(EstadoAnimal estado)
    {
        var ranchoId = GetRanchoId();

        var animales = _context.Animales
            .AsNoTracking()
            .Where(a => a.RanchoId == ranchoId && a.Estado == estado)
            .Select(a => new
            {
                a.AnimalId,
                a.NumeroArete,
                a.Nombre,
                a.Sexo,
                a.Raza,
                a.Estado,
                a.PotreroId
            })
            .ToList();

        return Ok(animales);
    }

    [HttpPatch("{id}/mover-potrero")]
    public async Task<IActionResult> MoverAnimalPotrero(int id, AnimalMoverPotreroDto dto)
    {
        var ranchoId = GetRanchoId();

        var animal = _context.Animales
            .FirstOrDefault(a => a.AnimalId == id && a.RanchoId == ranchoId);

        if (animal == null)
            return NotFound("Animal no encontrado");

        var potreroDestino = _context.Potreros
            .FirstOrDefault(p => p.PotreroId == dto.PotreroDestinoId && p.RanchoId == ranchoId);

        if (potreroDestino == null)
            return BadRequest("Potrero destino no válido");

        var potreroOrigenId = animal.PotreroId;

        animal.PotreroId = dto.PotreroDestinoId;

        _context.AnimalMovimientoPotrero.Add(new AnimalMovimientoPotrero
        {
            AnimalId = animal.AnimalId,
            PotreroOrigenId = potreroOrigenId,
            PotreroDestinoId = dto.PotreroDestinoId,
            FechaMovimiento = DateTime.UtcNow,
            Motivo = dto.Motivo ?? "Movimiento manual"
        });

        await _context.SaveChangesAsync();

        return Ok("Animal movido correctamente");
    }

    [HttpGet("{id}/movimientos")]
    public IActionResult ObtenerMovimientos(int id)
    {
        var ranchoId = GetRanchoId();

        var animalExiste = _context.Animales
            .Any(a => a.AnimalId == id && a.RanchoId == ranchoId);

        if (!animalExiste)
            return NotFound("Animal no encontrado");

        var movimientos = _context.AnimalMovimientoPotrero
            .AsNoTracking()
            .Where(m => m.AnimalId == id)
            .OrderByDescending(m => m.FechaMovimiento)
            .Select(m => new
            {
                m.FechaMovimiento,
                m.Motivo,
                PotreroOrigen = m.PotreroOrigenId,
                PotreroDestino = m.PotreroDestinoId
            })
            .ToList();

        return Ok(movimientos);
    }

    [HttpGet("{id}/estados")]
    public IActionResult ObtenerHistorialEstados(int id)
    {
        var ranchoId = GetRanchoId();

        var animalExiste = _context.Animales
            .Any(a => a.AnimalId == id && a.RanchoId == ranchoId);

        if (!animalExiste)
            return NotFound("Animal no encontrado");

        var historial = _context.AnimalEstadoHistorial
            .AsNoTracking()
            .Where(e => e.AnimalId == id)
            .OrderByDescending(e => e.FechaEstado)
            .Select(e => new
            {
                e.Estado,
                e.FechaEstado,
                e.Motivo,
                e.PrecioVenta
            })
            .ToList();

        return Ok(historial);
    }
}
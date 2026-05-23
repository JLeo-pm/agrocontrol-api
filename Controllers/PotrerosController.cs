using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRancho.API.Data;
using SmartRancho.API.Dtos;
using SmartRancho.API.Entities;

namespace SmartRancho.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PotrerosController : ControllerBase
{
    private readonly SmartRanchoDbContext _context;

    public PotrerosController(SmartRanchoDbContext context)
    {
        _context = context;
    }

    private int GetRanchoId()
    {
        return int.Parse(User.FindFirst("RanchoId")?.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(PotreroCreateDto dto)
    {
        var potrero = new Potrero
        {
            RanchoId = GetRanchoId(),
            Nombre = dto.Nombre,
            TamanoHectareas = dto.TamanoHectareas,
            Activo = true
        };

        _context.Potreros.Add(potrero);
        await _context.SaveChangesAsync();

        return Ok(potrero);
    }

    [HttpGet]
    public IActionResult Listar()
    {
        var ranchoId = GetRanchoId();

        var potreros = _context.Potreros
            .AsNoTracking()
            .Where(p => p.RanchoId == ranchoId && p.Activo)
            .Select(p => new
            {
                p.PotreroId,
                p.Nombre,
                p.TamanoHectareas
            })
            .ToList();

        return Ok(potreros);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Editar(int id, PotreroCreateDto dto)
    {
        var ranchoId = GetRanchoId();

        var potrero = await _context.Potreros
            .FirstOrDefaultAsync(p => p.PotreroId == id && p.RanchoId == ranchoId);

        if (potrero == null)
            return NotFound("Potrero no encontrado");

        potrero.Nombre = dto.Nombre;
        potrero.TamanoHectareas = dto.TamanoHectareas;

        await _context.SaveChangesAsync();

        return Ok(potrero);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Desactivar(int id)
    {
        var ranchoId = GetRanchoId();

        var potrero = await _context.Potreros
            .FirstOrDefaultAsync(p => p.PotreroId == id && p.RanchoId == ranchoId);

        if (potrero == null)
            return NotFound("Potrero no encontrado");

        potrero.Activo = false;
        await _context.SaveChangesAsync();

        return Ok("Potrero desactivado correctamente");
    }
}
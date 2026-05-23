using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRancho.API.Data;
using SmartRancho.API.Dtos;
using SmartRancho.API.Entities.Enums;

namespace SmartRancho.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly SmartRanchoDbContext _context;

    public DashboardController(SmartRanchoDbContext context)
    {
        _context = context;
    }

    private int GetRanchoId()
    {
        return int.Parse(User.FindFirst("RanchoId")?.Value);
    }

    [HttpGet]
    public IActionResult ObtenerDashboard()
    {
        var ranchoId = GetRanchoId();

        var animales = _context.Animales
            .Where(a => a.RanchoId == ranchoId);

        var dashboard = new DashboardDto
        {
            TotalAnimales = animales.Count(),

            Activos = animales.Count(a => a.Estado == EstadoAnimal.Activo),
            Vendidos = animales.Count(a => a.Estado == EstadoAnimal.Vendido),
            Muertos = animales.Count(a => a.Estado == EstadoAnimal.Muerto),
            Perdidos = animales.Count(a => a.Estado == EstadoAnimal.Perdido),

            TotalPotreros = _context.Potreros.Count(p => p.RanchoId == ranchoId && p.Activo),

            AnimalesPorPotrero = _context.Potreros
                .Where(p => p.RanchoId == ranchoId && p.Activo)
                .Select(p => new AnimalesPorPotreroDto
                {
                    Potrero = p.Nombre,
                    Cantidad = p.Animales.Count(a => a.Estado == EstadoAnimal.Activo)
                })
                .ToList()
        };

        return Ok(dashboard);
    }
}
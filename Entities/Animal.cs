using SmartRancho.API.Entities.Enums;

namespace SmartRancho.API.Entities;

public class Animal
{
    public int AnimalId { get; set; }
    public int RanchoId { get; set; }
    public Rancho Rancho { get; set; }

    public int? PotreroId { get; set; }
    public Potrero? Potrero { get; set; }

    public string NumeroArete { get; set; }
    public string? Nombre { get; set; }
    public string? Sexo { get; set; }
    public string? Raza { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public string? Color { get; set; }
    public string? FotoUrl { get; set; }
    public string? Observaciones { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public List<AnimalEstadoHistorial> Estados { get; set; } = new();

    public EstadoAnimal Estado { get; set; } = EstadoAnimal.Activo;
    public List<AnimalMovimientoPotrero> MovimientosPotrero { get; set; } = new();
}
namespace SmartRancho.API.Entities;

using SmartRancho.API.Entities.Enums;

public class AnimalEstadoHistorial
{
    public int Id { get; set; }

    public int AnimalId { get; set; }
    public Animal? Animal { get; set; }

    public EstadoAnimal Estado { get; set; }

    public DateTime FechaEstado { get; set; }

    public string? Motivo { get; set; }

    public decimal? PrecioVenta { get; set; }
}
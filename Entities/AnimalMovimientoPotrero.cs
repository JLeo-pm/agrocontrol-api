namespace SmartRancho.API.Entities;

public class AnimalMovimientoPotrero
{
    public int AnimalMovimientoPotreroId { get; set; }

    public int AnimalId { get; set; }
    public Animal Animal { get; set; }
    public int? PotreroOrigenId { get; set; }
    public Potrero? PotreroOrigen { get; set; }
    public int PotreroDestinoId { get; set; }
    public Potrero PotreroDestino { get; set; }

    public DateTime FechaMovimiento { get; set; } = DateTime.UtcNow;

    public string? Motivo { get; set; }
}
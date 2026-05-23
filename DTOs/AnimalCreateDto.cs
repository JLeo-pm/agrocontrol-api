namespace SmartRancho.API.Dtos;

public class AnimalCreateDto
{
    public string NumeroArete { get; set; }
    public string? Nombre { get; set; }
    public string? Sexo { get; set; }
    public string? Raza { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public string? Color { get; set; }
    public string? Observaciones { get; set; }
    public int? PotreroId { get; set; }
}
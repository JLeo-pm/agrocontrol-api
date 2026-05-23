namespace SmartRancho.API.Entities;

public class Potrero
{
    public int PotreroId { get; set; }
    public int RanchoId { get; set; }
    public Rancho Rancho { get; set; }

    public string Nombre { get; set; }
    public decimal? TamanoHectareas { get; set; }
    public bool Activo { get; set; } = true;

    public List<Animal> Animales { get; set; }
}
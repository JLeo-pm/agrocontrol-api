namespace SmartRancho.API.Entities;

public class Rancho
{
    public int RanchoId { get; set; }
    public string NombreRancho { get; set; }
    public string? Propietario { get; set; }
    public string? EmailContacto { get; set; }
    public string? Telefono { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    public bool Activo { get; set; } = true;

    public List<Usuario> Usuarios { get; set; }
    public List<Potrero> Potreros { get; set; }
    public List<Animal> Animales { get; set; }
}
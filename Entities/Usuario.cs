namespace SmartRancho.API.Entities;

public class Usuario
{
    public int UsuarioId { get; set; }
    public int RanchoId { get; set; }
    public Rancho Rancho { get; set; }

    public string Nombre { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Rol { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
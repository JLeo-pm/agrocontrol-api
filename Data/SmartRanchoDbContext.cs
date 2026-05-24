using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartRancho.API.Entities;
using SmartRancho.API.Auth;

namespace SmartRancho.API.Data;

public class SmartRanchoDbContext : IdentityDbContext<ApplicationUser>
{
    public SmartRanchoDbContext(DbContextOptions<SmartRanchoDbContext> options)
        : base(options) { }

    public DbSet<Rancho> Ranchos => Set<Rancho>();
    public DbSet<Potrero> Potreros => Set<Potrero>();
    public DbSet<Animal> Animales => Set<Animal>();
    public DbSet<AnimalEstadoHistorial> AnimalEstadoHistorial { get; set; }
    public DbSet<AnimalMovimientoPotrero> AnimalMovimientoPotrero => Set<AnimalMovimientoPotrero>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AnimalMovimientoPotrero>()
            .HasOne(m => m.PotreroOrigen)
            .WithMany()
            .HasForeignKey(m => m.PotreroOrigenId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AnimalMovimientoPotrero>()
            .HasOne(m => m.PotreroDestino)
            .WithMany()
            .HasForeignKey(m => m.PotreroDestinoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AnimalMovimientoPotrero>()
            .HasOne(m => m.Animal)
            .WithMany(a => a.MovimientosPotrero)
            .HasForeignKey(m => m.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Animal>()
            .HasIndex(a => new { a.RanchoId, a.NumeroArete })
            .IsUnique()
            .HasDatabaseName("UX_Animal_Rancho_Arete");

        modelBuilder.Entity<AnimalEstadoHistorial>()
            .Property(e => e.PrecioVenta)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Potrero>()
            .Property(p => p.TamanoHectareas)
            .HasPrecision(10, 2);
    }
}
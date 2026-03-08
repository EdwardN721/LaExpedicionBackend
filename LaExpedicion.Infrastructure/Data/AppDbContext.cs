using System.Reflection;
using LaExpedicion.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LaExpedicion.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<Usuario, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Estadistica> Estadisticas { get; set; }
    public DbSet<Etiqueta> Etiquetas { get; set; }
    public DbSet<Expedicion> Expediciones { get; set; }
    public DbSet<ExpedicionRealizada> ExpedicionRealizadas { get; set; }
    public DbSet<Inventario> Inventarios { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemModificador> ItemModificadores { get; set; }
    public DbSet<Personaje> Personajes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}
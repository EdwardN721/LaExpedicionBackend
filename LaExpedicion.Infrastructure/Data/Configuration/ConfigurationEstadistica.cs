using LaExpedicion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaExpedicion.Infrastructure.Data.Configuration;

public class ConfigurationEstadistica : IEntityTypeConfiguration<Estadistica>
{
    public void Configure(EntityTypeBuilder<Estadistica> builder)
    {
        // Nombre de la tabla en SQL Server
        builder.ToTable("Estadisticas");
        
        // Configuración de la llame Primaria y 
        // generación de UUID 
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();
        
        // Reglas de las columnas
        
        builder.Property(e => e.Energia)
            .IsRequired();
        builder.Property(e => e.Fuerza)
            .IsRequired();
        builder.Property(e => e.Magia)
            .IsRequired();
        builder.Property(e => e.Salud)
            .IsRequired();
        builder.Property(e => e.Mana)
            .IsRequired();
        
        // Configuracion de la relacion
        builder.HasOne(e => e.Personaje)
            .WithOne(e => e.Estadistica)
            .HasForeignKey<Estadistica>(e => e.PersonajeId) // En 1 a 1, hay que decirle a EF qué tabla lleva la llave
            .OnDelete(DeleteBehavior.Cascade); // Si borro el personaje, borro sus stats
    }
}
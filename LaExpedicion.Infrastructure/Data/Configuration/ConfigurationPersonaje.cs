using LaExpedicion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaExpedicion.Infrastructure.Data.Configuration;

public class ConfigurationPersonaje : IEntityTypeConfiguration<Personaje>
{
    public void Configure(EntityTypeBuilder<Personaje> builder)
    {
        // Nombre de la tabla en SQL Server
        builder.ToTable("Personajes");
        
        // Configuración de la llame Primaria y 
        // generación de UUID 
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(p => p.NombreUsuario)
            .IsRequired()
            .HasMaxLength(20);
        
        // Progresion del personaje
        builder.Property(p => p.Nivel).IsRequired().HasDefaultValue(1);
        builder.Property(p => p.Experiencia).IsRequired().HasDefaultValue(0);
        builder.Property(p => p.Dinero).IsRequired().HasDefaultValue(0.0);
        builder.Property(p => p.SaludActual).IsRequired();
        
        // Configuracion relaciones
        // Relación 1 a Muchos: Un Usuario (Identity) puede tener Muchos Personajes
        builder.HasOne(p => p.Usuario)
            .WithMany() // Se queda vacío porque en la clase Usuario no pusimos una lista de personajes
            .HasForeignKey(i => i.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Relación 1 a Muchos: Una Etiqueta la tienen Muchos Personajes
        builder.HasOne(p => p.Etiqueta)
            .WithMany(e => e.Personajes)
            .HasForeignKey(i => i.EtiquetaId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(p => p.Inventario)
            .WithOne(i => i.Personaje)
            .HasForeignKey(p => p.PersonajeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
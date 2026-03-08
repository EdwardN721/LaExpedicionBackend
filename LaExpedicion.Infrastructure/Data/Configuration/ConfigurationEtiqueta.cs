using LaExpedicion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaExpedicion.Infrastructure.Data.Configuration;

public class ConfigurationEtiqueta : IEntityTypeConfiguration<Etiqueta>
{
    public void Configure(EntityTypeBuilder<Etiqueta> builder)
    {
        // Nombre de la tabla en SQL Server
        builder.ToTable("Etiquetas");
        
        // Configuración de la llame Primaria y 
        // generación de UUID 
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();
        
        // Reglas de las columnas
        builder.Property(e => e.Nombre)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(e => e.Descripcion)
            .HasMaxLength(150);
            
        // Configuración de la Relación (1 a Muchos con Modificadores)
        builder.HasMany(p => p.Personajes)
            .WithOne(e => e.Etiqueta)
            .HasForeignKey(p => p.EtiquetaId)
            .OnDelete(DeleteBehavior.Restrict); // Si borras el Item, se borran sus modificadores
    }
}
using LaExpedicion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaExpedicion.Infrastructure.Data.Configuration;

public class ConfigurationExpedition : IEntityTypeConfiguration<Expedicion>
{
    public void Configure(EntityTypeBuilder<Expedicion> builder)
    {
        
        // Nombre de la tabla en SQL Server
        builder.ToTable("Expediciones");
        
        // Configuración de la llame Primaria y 
        // generación de UUID 
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(e => e.Nombre)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(e => e.Descripcion)
            .HasMaxLength(200);
        builder.Property(e => e.Dinero)
            .IsRequired()
            .HasPrecision(14, 2);
        builder.Property(e => e.Experiencia)
            .IsRequired();

        // Configurar relacion
        builder.HasMany(e => e.ExpedicionesRealizadas)
            .WithOne(a => a.Expedicion)
            .HasForeignKey(e => e.ExpedicionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
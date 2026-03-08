using LaExpedicion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaExpedicion.Infrastructure.Data.Configuration;

public class ConfigurationExpedicionesRealizadas : IEntityTypeConfiguration<ExpedicionRealizada>
{
    public void Configure(EntityTypeBuilder<ExpedicionRealizada> builder)
    {
        // Nombre de la tabla en SQL Server
        builder.ToTable("ExpedicionRealizadas");
        
        // Configuración de la llame Primaria y 
        // generación de UUID 
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.FechaInicio)
            .IsRequired();
        builder.Property(e => e.Resultado)
            .IsRequired()
            .HasConversion<string>();
        builder.Property(e => e.DineroGanado)
            .IsRequired()
            .HasPrecision(14, 2);
        
        // Configurar relaciones

        builder.HasOne(d => d.Expedicion)
            .WithMany(p => p.ExpedicionesRealizadas)
            .HasForeignKey(d => d.ExpedicionId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(d => d.Personaje)
            .WithMany()
            .HasForeignKey(d => d.PersonajeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
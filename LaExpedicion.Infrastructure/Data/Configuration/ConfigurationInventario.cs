using LaExpedicion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaExpedicion.Infrastructure.Data.Configuration;

public class ConfigurationInventario : IEntityTypeConfiguration<Inventario>
{
    public void Configure(EntityTypeBuilder<Inventario> builder)
    {
        // Nombre de la tabla en SQL Server
        builder.ToTable("Inventario");
        
        // Configuración de la llame Primaria y 
        // generación de UUID 
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();
        
      builder.Property(i => i.Equipado)
            .IsRequired();
        builder.Property(i => i.UsosRestantes)
            .IsRequired()
            .HasDefaultValue(1);
        
        // Configurar relaciones
        // Un Item puede estar en MUCHOS inventarios
        builder.HasOne(i => i.Item)
            .WithMany(item => item.Inventario)
            .HasForeignKey(i => i.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Un Personaje tiene MUCHOS slots
        builder.HasOne(i => i.Personaje)
            .WithMany(p => p.Inventario)
            .HasForeignKey(i => i.PersonajeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
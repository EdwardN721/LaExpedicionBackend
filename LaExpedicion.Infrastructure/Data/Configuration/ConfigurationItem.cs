using LaExpedicion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaExpedicion.Infrastructure.Data.Configuration;

public class ConfigurationItem : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        // Nombre de la tabla en SQL Server
        builder.ToTable("Items");
        
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
        
        builder.HasMany(im => im.ItemModificador)
            .WithOne(i => i.Item)
            .HasForeignKey(i => i.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(i => i.Inventario)
            .WithOne(i => i.Item)
            .HasForeignKey(i => i.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
            
    }
}
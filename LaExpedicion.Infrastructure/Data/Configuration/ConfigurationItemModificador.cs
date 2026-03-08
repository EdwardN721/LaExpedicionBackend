using LaExpedicion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaExpedicion.Infrastructure.Data.Configuration;

public class ConfigurationItemModificador : IEntityTypeConfiguration<ItemModificador>
{
    public void Configure(EntityTypeBuilder<ItemModificador> builder)
    {
        // Nombre de la tabla en SQL Server
        builder.ToTable("ItemsModificadores");
        
        // Configuración de la llame Primaria y 
        // generación de UUID 
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(im => im.ItemId)
            .IsRequired();
        builder.Property(im => im.EstadisticaAfectada)
            .IsRequired()
            .HasConversion<string>();
        builder.Property(im => im.ValorAjuste)
            .IsRequired()
            .HasPrecision(0, 5);
        
        // Configuracion relacion
        builder.HasOne(im => im.Item)
            .WithMany(i => i.ItemModificador)
            .HasForeignKey(im => im.ItemId);
    }
}
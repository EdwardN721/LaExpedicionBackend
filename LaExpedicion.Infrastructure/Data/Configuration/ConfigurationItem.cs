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
        
        builder.Property(i => i.Precio)
            .IsRequired()
            .HasDefaultValue(50.0); // Precio base
        
        builder.HasMany(im => im.ItemModificador)
            .WithOne(i => i.Item)
            .HasForeignKey(i => i.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(i => i.Inventario)
            .WithOne(i => i.Item)
            .HasForeignKey(i => i.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
            
        // Semilla 
        builder.HasData(
            new Item 
            { 
                Id = Guid.Parse("f19dd3d1-3837-4a9a-80d1-190a8f128d9c"), 
                Nombre = "Hierba Verde", 
                Descripcion = "Planta medicinal clásica que restaura una cantidad moderada de salud. Mejor si se combina.",
                Precio = 50.0
            },
            new Item 
            { 
                Id = Guid.Parse("0c336332-db1e-4308-a678-e5309c42a795"), 
                Nombre = "Espada Maestra", 
                Descripcion = "La hoja destructora del mal, forjada por diosas antiguas.",
                Precio = 10000.0
            },
            new Item 
            { 
                Id = Guid.Parse("aaef9db3-a67f-46f8-aec7-e37355d454e2"), 
                Nombre = "Poción Felix Felicis", 
                Descripcion = "Suerte líquida. Quien la bebe tendrá éxito en todo lo que intente.",
                Precio = 25000.0
            },
            new Item 
            { 
                Id = Guid.Parse("1e2dfb2e-9264-4d8f-8a7e-b9a174bb1192"), 
                Nombre = "Spray de Primeros Auxilios", 
                Descripcion = "Cilindro presurizado que restaura la salud por completo de forma casi instantánea." ,
                Precio = 150.0
            },
            new Item 
            { 
                Id = Guid.Parse("ef70e69f-798f-4923-a8a2-3d88d719eb0f"), 
                Nombre = "Sable de Luz", 
                Descripcion = "Un arma elegante para una era más civilizada. Capaz de cortar casi cualquier material.",
                Precio = 12000.0
            },
            new Item 
            { 
                Id = Guid.Parse("7ea6bd92-05ba-4dea-899d-408e95d28031"), 
                Nombre = "El Anillo Único", 
                Descripcion = "Otorga invisibilidad a su portador, pero corrompe su mente lentamente.",
                Precio = 50000.0
            },
            new Item 
            { 
                Id = Guid.Parse("b5e4edfa-3c3f-4a77-83e7-b34498148df3"), 
                Nombre = "Lanzaredes", 
                Descripcion = "Dispositivo de muñeca que dispara un fluido sintético con la resistencia del acero.",
                Precio = 1000.0
            }
        );
    }
}
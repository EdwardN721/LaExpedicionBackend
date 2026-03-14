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
        
        // Semilla
        builder.HasData(
            new Etiqueta 
            { 
                Id = Guid.Parse("f19dd3d1-3837-4a9a-80d1-190a8f128d9c"), 
                Nombre = "Superviviente", 
                Descripcion = "Especialista en salir con vida de situaciones biológicas extremas." 
            },
            new Etiqueta 
            { 
                Id = Guid.Parse("0c336332-db1e-4308-a678-e5309c42a795"), 
                Nombre = "Brujo", 
                Descripcion = "Mutante entrenado para rastrear y cazar monstruos a cambio de monedas." 
            },
            new Etiqueta 
            { 
                Id = Guid.Parse("aaef9db3-a67f-46f8-aec7-e37355d454e2"), 
                Nombre = "Jedi", 
                Descripcion = "Guardián de la paz y la justicia, sensible a fuerzas místicas." 
            },
            new Etiqueta 
            { 
                Id = Guid.Parse("2a3dfde7-ce67-4205-8f0f-08f3d6287a0b"), 
                Nombre = "Agente S.T.A.R.S.", 
                Descripcion = "Especialista en tácticas de rescate, armas de fuego y supervivencia extrema." 
            },
            new Etiqueta 
            { 
                Id = Guid.Parse("e8f3cbb0-78a4-4001-85d1-9d923019ed86"), 
                Nombre = "Asesino de la Hermandad", 
                Descripcion = "Maestro del sigilo, el parkour y el uso de la hoja oculta bajo las sombras." 
            },
            new Etiqueta 
            { 
                Id = Guid.Parse("D1111111-1111-1111-1111-111111111111"), 
                Nombre = "Novato", // Promedio 1 - 3 
                Descripcion = "Héroe con atributos muy bajos, ¿estará destinado para apuntar a lo grande?" 
            },
            new Etiqueta 
            { 
                Id = Guid.Parse("D2222222-2222-2222-2222-222222222222"), 
                Nombre = "Aventurero", // Promedio 4 - 6
                Descripcion = "Un combatiente estándar y capaz, listo para forjar su propio destino en este peligroso mundo." 
            },
            new Etiqueta 
            { 
                Id = Guid.Parse("D3333333-3333-3333-3333-333333333333"), 
                Nombre = "Talento Nato", // Promedio 7 - 9
                Descripcion = "Destaca entre la multitud. Sus habilidades sobresalientes le auguran un futuro brillante y lleno de victorias." 
            },
            new Etiqueta 
            { 
                Id = Guid.Parse("D4444444-4444-4444-4444-444444444444"), 
                Nombre = "Genio", // Promedio 10
                Descripcion = "Héroe imparable, hay muchas expectativas puestas sobre ti. El mundo entero observa tus pasos." 
            }
        );
    }
}
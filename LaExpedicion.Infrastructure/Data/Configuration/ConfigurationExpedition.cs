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
        
        // Semilla
        builder.HasData(
            new Expedicion 
            { 
                Id = Guid.Parse("64fea296-23f9-430e-bec4-f5d01dc86509"), 
                Nombre = "Incidente en la Mansión Spencer", 
                Descripcion = "Investiga los extraños incidentes y desapariciones en las oscuras montañas Arklay.", 
                Experiencia = 500, 
                Dinero = 1500.00 
            },
            new Expedicion 
            { 
                Id = Guid.Parse("83262a7b-6916-4ec4-b4a6-bb62cbfee247"), 
                Nombre = "Viaje a Mordor", 
                Descripcion = "Una peligrosa travesía para destruir un objeto de inmenso poder en los fuegos del Monte del Destino.", 
                Experiencia = 2000, 
                Dinero = 0.00 // ¡El honor es la verdadera recompensa!
            },
            new Expedicion 
            { 
                Id = Guid.Parse("65fcf63f-94fb-4f1a-abb6-004a02b078c3"), 
                Nombre = "Escape de Raccoon City", 
                Descripcion = "Abrete paso entre hordas de infectados y mutantes antes de que la ciudad sea purgada del mapa.", 
                Experiencia = 1500, 
                Dinero = 500.00 
            },
            new Expedicion 
            { 
                Id = Guid.Parse("067cdc49-de25-4e88-bec6-42f9aa627dba"), 
                Nombre = "Defensa de Invernalia", 
                Descripcion = "Resiste el asedio del Rey de la Noche y su ejército de caminantes blancos.", 
                Experiencia = 3000, 
                Dinero = 2500.00 
            },
            new Expedicion 
            { 
                Id = Guid.Parse("C3333333-3333-3333-3333-333333333333"), 
                Nombre = "Torneo de los Tres Magos", 
                Descripcion = "Sobrevive a dragones, rescata rehenes en el lago negro y encuentra la copa en el laberinto.", 
                Experiencia = 2500, 
                Dinero = 10000.00 
            }
        );
    }
}
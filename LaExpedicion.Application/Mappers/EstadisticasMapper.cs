using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Domain.Entities;

namespace LaExpedicion.Application.Mappers;

public static class EstadisticasMapper
{
    public static Estadistica MapToEntity(this CrearEstadisticaDto dto)
    {
        return new Estadistica
        {
            PersonajeId = dto.PersonajeId,
            Salud = dto.Salud,
            Fuerza = dto.Fuerza,
            Energia = dto.Energia,
            Magia = dto.Magia,
            Mana = dto.Mana
        };
    }

    public static void UpdateEntity(this Estadistica entity, ActualizarEstadisticaDto dto)
    {
        entity.Salud = dto.Salud;
        entity.Fuerza = dto.Fuerza;
        entity.Energia = dto.Energia;
        entity.Magia = dto.Magia;
        entity.Mana = dto.Mana;
    }

    public static EstadisticaDto MapToDto(this Estadistica entity)
    {
        return new EstadisticaDto
        {
            Id = entity.Id,
            PersonajeId = entity.PersonajeId,
            Salud = entity.Salud,
            Fuerza = entity.Fuerza,
            Energia = entity.Energia,
            Magia = entity.Magia,
            Mana = entity.Mana
        };
    }

    public static IEnumerable<EstadisticaDto> MapToDto(this IEnumerable<Estadistica>? entities)
    {
        return entities?.Select(MapToDto) ?? Enumerable.Empty<EstadisticaDto>();
    }
}
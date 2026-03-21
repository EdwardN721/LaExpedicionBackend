using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Domain.Entities;

namespace LaExpedicion.Application.Mappers;

public static class ExpedicionMapper
{
    public static Expedicion MapToEntity(this CrearExpedicionDto dto)
    {
        return new Expedicion
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion ?? "Sin Descripcion.",
            Dinero = dto.Dinero,
            Experiencia = dto.Experiencia,
        };
    }

    public static void UpdateEntity(this Expedicion entity, ActualizarExpedicionDto dto)
    {
        entity.Nombre = dto.Nombre;
        entity.Descripcion = dto.Descripcion;
        entity.Dinero = dto.Dinero;
        entity.Experiencia = dto.Experiencia;
    }

    public static ExpedicionDto MapToDto(this Expedicion expedicion)
    {
        return new ExpedicionDto
        {
            Id = expedicion.Id,
            Nombre = expedicion.Nombre,
            Descripcion = expedicion.Descripcion,
            Dinero = expedicion.Dinero,
            Experiencia = expedicion.Experiencia,
        };
    }

    public static IEnumerable<ExpedicionDto> MapToDto(this IEnumerable<Expedicion>? expediciones)
    {
        return expediciones?.Select(MapToDto) ?? Enumerable.Empty<ExpedicionDto>();
    }
}
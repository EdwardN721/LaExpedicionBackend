using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Domain.Entities;

namespace LaExpedicion.Application.Mappers;

public static class EtiquetaMapper
{
    public static Etiqueta MapToEntity(this CrearEtiquetaDto dto)
    {
        return new Etiqueta
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion ?? "Sin Descripción.",
        };
    }

    public static void UpdateEntity(this Etiqueta entity, ActualizarEtiquetaDto dto)
    {
        entity.Nombre = dto.Nombre;
        entity.Descripcion = dto.Descripcion;
    }
    
    public static EtiquetaDto MapToDto(this Etiqueta entity)
    {
        return new EtiquetaDto
        {
            Id = entity.Id,
            Nombre = entity.Nombre,
            Descripcion = entity.Descripcion
        };
    }

    public static IEnumerable<EtiquetaDto> MapToDto(this IEnumerable<Etiqueta>? entities)
    {
        return entities?.Select(MapToDto) ?? Enumerable.Empty<EtiquetaDto>();
    }
}
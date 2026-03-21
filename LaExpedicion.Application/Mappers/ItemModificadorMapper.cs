using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Domain.Entities;

namespace LaExpedicion.Application.Mappers;

public static class ItemModificadorMapper
{
    public static ItemModificador MapToEntity(this CrearItemModificadorDto dto)
    {
        return new ItemModificador
        {
            EstadisticaAfectada = dto.EstadisticaAfectada,
            ValorAjuste = dto.ValorAjustado
        };
    }

    public static ItemModificadorDto MapToDto(this ItemModificador entity)
    {
        return new ItemModificadorDto
        {
            Id = entity.Id,
            ItemId = entity.ItemId,
            EstadisticaAfectada = entity.EstadisticaAfectada.ToString(),
            ValorAjuste = entity.ValorAjuste
        };
    }

    public static IEnumerable<ItemModificadorDto> MapToDto(this IEnumerable<ItemModificador>? entities)
    {
        return entities?.Select(MapToDto) ?? Enumerable.Empty<ItemModificadorDto>();
    }
}
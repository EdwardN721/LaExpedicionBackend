using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Domain.Entities;
using LaExpedicion.Domain.Enum;

namespace LaExpedicion.Application.Mappers;

public static class ItemMapper
{
    public static Item MapToEntity(this CrearItemDto dto)
    {
        return new Item
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion ?? "Sin Descripción.",
            Precio = dto.Precio,
            TipoItem = (EnumTipoItems)dto.TipoItem,
        };
    }

    public static void UpdateEntity(this Item entity, ActualizarItemDto dto)
    {
        entity.Nombre = dto.Nombre;
        entity.Descripcion = dto.Descripcion;
        entity.Precio = dto.Precio;
        entity.TipoItem = (EnumTipoItems)dto.TipoItem;
        entity.Activo = dto.Activo;
    }

    public static ItemDto MapToDto(this Item item)
    {
        return new ItemDto
        {
            Id = item.Id,
            Nombre = item.Nombre,
            Descripcion = item.Descripcion ?? "Sin Descripción.",
            Precio = item.Precio,
            TipoItem = item.TipoItem,
            ItemModificador = item.ItemModificador.Select(im => im.MapToDto()).ToList(),
            ImagenUrl = item.ImagenUrl ?? "Sin Imagen.",
            Activo = item.Activo
        };
    }

    public static IEnumerable<ItemDto> MapToDto(this IEnumerable<Item>? items)
    {
        if (items == null) return Enumerable.Empty<ItemDto>();
        return items.Select(MapToDto);
    }
}
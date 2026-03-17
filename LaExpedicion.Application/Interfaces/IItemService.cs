using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Parameters;
using LaExpedicion.Shared.Pagination;

namespace LaExpedicion.Application.Interfaces;

public interface IItemService
{
    Task<PagedList<ItemDto>> ObtenerTodosItems(ItemParameters itemParameters);
    Task<ItemDto> ObtenerItemPorId(Guid id);
    Task<ItemDto> CrearItem(CrearItemDto item);
    Task ModificarItem(Guid id, ActualizarItemDto item);
    Task EliminarItem(Guid id);
}
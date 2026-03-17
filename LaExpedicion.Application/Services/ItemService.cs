using System.Linq.Expressions;
using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Interfaces;
using LaExpedicion.Application.Mappers;
using LaExpedicion.Application.Parameters;
using LaExpedicion.Domain.Entities;
using LaExpedicion.Domain.Exceptions;
using LaExpedicion.Domain.Interfaces;
using LaExpedicion.Shared.Pagination;
using Microsoft.Extensions.Logging;

namespace LaExpedicion.Application.Services;

public class ItemService : IItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ItemService> _logger;

    public ItemService(IUnitOfWork unitOfWork, ILogger<ItemService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ??  throw new ArgumentNullException(nameof(logger));
    }

    public async Task<PagedList<ItemDto>> ObtenerTodosItems(ItemParameters itemParameters)
    {
        Expression<Func<Item, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(itemParameters.Nombre))
        {
            // Entity framework lo traducirá a un LIKE '%nombre%' en SQL
            filter = x => x.Nombre.Contains(itemParameters.Nombre);
        }
        
        var (registros, total) = await _unitOfWork.Items.ObtenerPaginadosAsync(
            filter,
            itemParameters.PageNumber,
            itemParameters.PageSize);
        
        List<ItemDto> items = registros.Select(item => item.MapToDto()).ToList();
        
        return new PagedList<ItemDto>(
            items, total, itemParameters.PageNumber, itemParameters.PageSize);
    }

    public async Task<ItemDto> ObtenerItemPorId(Guid id)
    {
        Item item = await ObtenerPorId(id);

        return item.MapToDto();
    }

    public async Task<ItemDto> CrearItem(CrearItemDto item)
    {
        _logger.LogInformation("Creando nuevo item: {NombreItem}", item.Nombre);
        Item nuevoItem = item.MapToEntity();
        
        await _unitOfWork.Items.AgregarAsync(nuevoItem);
        await _unitOfWork.SaveChangesAsync();
        
        _logger.LogInformation("Item creado exitosamente con Id: {Id}", nuevoItem.Id);
        return nuevoItem.MapToDto();
    }

    public async Task ModificarItem(Guid id, ActualizarItemDto itemDto)
    {
        Item item = await ObtenerPorId(id);
        
        item.UpdateEntity(itemDto);
        
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Item modificado correctamente con Id: {Id}", id);
    }

    public async Task EliminarItem(Guid id)
    {
        Item item = await ObtenerPorId(id);
        
        _unitOfWork.Items.Eliminar(item);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogWarning("Item eliminado correctamente con Id: {Id}", id);
    }

    #region MetodosPrivados

    private async Task<Item> ObtenerPorId(Guid id)
    {
        Item? item = await _unitOfWork.Items.ObtenerPorIdAsync(id);

        if (item == null)
        {
            _logger.LogWarning("No existe el item con Id: {Id}", id);
            throw new NotFoundException($"No existe el item con Id: {id}");
        }
        
        return item;
    }

    #endregion
}
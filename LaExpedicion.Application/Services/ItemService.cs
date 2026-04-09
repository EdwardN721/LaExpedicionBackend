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
    private readonly IBlobStorageService _blobStorageService;

    public ItemService(IUnitOfWork unitOfWork, ILogger<ItemService> logger,
        IBlobStorageService blobStorageService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
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
            itemParameters.PageSize,
            x => x.ItemModificador
        );

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
        string? urlImagenFinal = null;
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            Item nuevoItem = item.MapToEntity();
            await _unitOfWork.Items.AgregarAsync(nuevoItem);
            await _unitOfWork.SaveChangesAsync();

            // Creamos una lista temporal para guardar los modificadores y asignarlos a la memoria RAM
            var modificadoresEnMemoria = new List<ItemModificador>();
            
            if (item.Modificadores != null && item.Modificadores.Any())
            {
                foreach (var modificador in item.Modificadores)
                {
                    ItemModificador nuevaModificacion = modificador.MapToEntity();
                    nuevaModificacion.ItemId = nuevoItem.Id;

                    await _unitOfWork.ItemModificadores.AgregarAsync(nuevaModificacion);
                    modificadoresEnMemoria.Add(nuevaModificacion); 
                }

                await _unitOfWork.SaveChangesAsync();
                
                nuevoItem.ItemModificador = modificadoresEnMemoria;
            }

            if (item.Imagen != null)
            {
                string nombreSeguro = item.Nombre.ToLower().Trim().Replace(" ", "-");
                nombreSeguro = System.Text.RegularExpressions.Regex.Replace(nombreSeguro, @"[^a-z0-9\-]", ""); // Evita problemas en acentos
                urlImagenFinal = await _blobStorageService.SubirArchivo(item.Imagen, "item-imagenes", nombreSeguro);
            }
            nuevoItem.ImagenUrl = urlImagenFinal;
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation("Item y sus modificadores creados exitosamente con Id: {Id}", nuevoItem.Id);

            return nuevoItem.MapToDto();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Error al crear el Item y sus modificadores");
            throw;
        }
    }

    public async Task ModificarItem(Guid id, ActualizarItemDto itemDto)
    {
        string? urlImagenFinal = null;

        Item item = await ObtenerPorId(id);

        item.UpdateEntity(itemDto);
        
        if (itemDto.Imagen != null)
        {
            string nombreSeguro = item.Nombre.ToLower().Trim().Replace(" ", "-");
            nombreSeguro = System.Text.RegularExpressions.Regex.Replace(nombreSeguro, @"[^a-z0-9\-]", ""); // Evita problemas en acentos
            urlImagenFinal = await _blobStorageService.SubirArchivo(itemDto.Imagen, "item-imagenes", nombreSeguro);
        }
        item.ImagenUrl = urlImagenFinal;

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
        Item? item = await _unitOfWork.Items.GetFirstOrDefaultAsync(
            i => i.Id == id,
            i => i.ItemModificador // JOIN
        );

        if (item == null)
        {
            _logger.LogWarning("No existe el item con Id: {Id}", id);
            throw new NotFoundException($"No existe el item con Id: {id}");
        }

        return item;
    }

    #endregion
}
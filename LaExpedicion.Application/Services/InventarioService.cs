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

public class InventarioService : IInventarioService
{
    private readonly IUnitOfWork _unitOfWork;
    private ILogger<InventarioService> _logger;

    public InventarioService(IUnitOfWork unitOfWork, ILogger<InventarioService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<PagedList<InventarioDto>> ObtenerInventarioDelPersonaje(Guid personajeId,
        InventarioParameters parametros)
    {
        Expression<Func<Inventario, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(parametros.Nombre))
        {
            filter = x => x.Item.Nombre.Contains(parametros.Nombre);
        }

        if (!parametros.Equipado)
        {
            filter = x => x.Equipado == parametros.Equipado;
        }

        var (registros, total) = await _unitOfWork.Inventarios.ObtenerPaginadosAsync(
            filter, 
            parametros.PageNumber,
            parametros.PageSize
            );
        
        List<InventarioDto> inventarios = registros.Select(inventario => inventario.MapToDto()).ToList();

        return new PagedList<InventarioDto>(
            inventarios,
            total,
            parametros.PageNumber,
            parametros.PageSize);
    }

    public async Task<InventarioDto> AgregarItem(CrearInventarioDto item)
    {
        _logger.LogInformation("Agregando item {ItemId} al personaje {PersonajeId}", item.ItemId, item.PersonajeId);
        Inventario nuevoRegistro = item.MapToEntity();
        
        await _unitOfWork.Inventarios.AgregarAsync(nuevoRegistro);
        await _unitOfWork.SaveChangesAsync();
        
        _logger.LogInformation("Item agregado al inventario exitosamente. Id de inventario: {Id}", nuevoRegistro.Id);
        return nuevoRegistro.MapToDto();
    }

    public async Task ActualizarInventario(Guid id, ActualizarInventarioDto item)
    {
        Inventario inventario = await ObtenerPorId(id);

        inventario.Equipado = item.Equipado;
        inventario.UsosRestantes = item.UsosRestantes;
        
        _unitOfWork.Inventarios.Actualizar(inventario);
        await _unitOfWork.SaveChangesAsync();
        
        _logger.LogInformation("Inventario {Id} actualizado. Equipado: {Equipado}, Usos: {Usos}", id, item.Equipado, item.UsosRestantes);
    }

    public async Task EliminarInventario(Guid id)
    {
        Inventario inventario = await ObtenerPorId(id);
        _unitOfWork.Inventarios.Eliminar(inventario);
        await _unitOfWork.SaveChangesAsync();
        
        _logger.LogInformation("El registro de inventario {Id} fue descartado/eliminado.", id);
    }

    public async Task UsarItem(Guid inventarioId, int usoAGastar = 1)
    {
        Inventario inventario = await ObtenerPorId(inventarioId);
        inventario.UsosRestantes -= usoAGastar;

        if (inventario.UsosRestantes <= 0)
        {
            _unitOfWork.Inventarios.Eliminar(inventario);
            _logger.LogInformation("El item {Id} se quedó sin usos (se rompió/gastó) y fue removido del inventario.", inventarioId);
        }
        else
        {
            _unitOfWork.Inventarios.Actualizar(inventario);
            _logger.LogInformation("Se usó el item {Id}. Usos restantes: {Usos}", inventarioId, inventario.UsosRestantes);
        }
        
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task EquiparItem(Guid inventarioId)
    {
        Inventario inventario = await ObtenerPorId(inventarioId);
        
        inventario.Equipado = !inventario.Equipado;
        
        _unitOfWork.Inventarios.Actualizar(inventario);
        await _unitOfWork.SaveChangesAsync();
        
        _logger.LogInformation("El item del inventario {Id} ahora tiene Equipado = {Estado}", inventarioId, inventario.Equipado);
    }

    #region MetodosPrivados

    private async Task<Inventario> ObtenerPorId(Guid id)
    {
        Inventario? inventario = await _unitOfWork.Inventarios.ObtenerPorIdAsync(id);

        if (inventario == null)
        {
            _logger.LogWarning("No existe ningun Inventario con el Id: {Id}", id);
            throw new NotFoundException($"No exíste el inventario con el Id: {id}");
        }
        
        return inventario;
    }

    #endregion
}
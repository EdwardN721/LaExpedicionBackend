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

public class ExpedicionService : IExpedicionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ExpedicionService> _logger;

    public ExpedicionService(IUnitOfWork unitOfWork, ILogger<ExpedicionService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<PagedList<ExpedicionDto>> ObtenerTodasExpediciones(ExpedicionParameters parameters)
    {
        Expression<Func<Expedicion, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(parameters.Nombre))
        {
            filter = x => x.Nombre.Contains(parameters.Nombre);
        }
        
        var (registros, total) = await _unitOfWork.Expediciones.ObtenerPaginadosAsync(
            filter,
            parameters.PageNumber,
            parameters.PageSize);

        List<ExpedicionDto> expediciones = registros.Select(expedicion => expedicion.MapToDto()).ToList();
        
        return new PagedList<ExpedicionDto>(
            expediciones,
            total,
            parameters.PageNumber,
            parameters.PageSize);
    }

    public async Task<ExpedicionDto> ObtenerExpedicionPorId(Guid id)
    {
        Expedicion expedicion = await ObtenerPorId(id);
        
        return expedicion.MapToDto();
    }

    public async Task<ExpedicionDto> CrearExpedicion(CrearExpedicionDto dto)
    {
        _logger.LogInformation("Creando nueva expedición: {Nombre}", dto.Nombre);
        
        Expedicion nuevaExpedicion = dto.MapToEntity();
        
        await _unitOfWork.Expediciones.AgregarAsync(nuevaExpedicion);
        await _unitOfWork.SaveChangesAsync();
        
        _logger.LogInformation("Expedición creada con éxito con Id: {Id}", nuevaExpedicion.Id);
        return nuevaExpedicion.MapToDto();
    }

    public async Task ActualizarExpedicion(Guid id, ActualizarExpedicionDto dto)
    {
        Expedicion expedicion = await ObtenerPorId(id);
        expedicion.UpdateEntity(dto); 
        
        _unitOfWork.Expediciones.Actualizar(expedicion);
        await _unitOfWork.SaveChangesAsync();
        
        _logger.LogInformation("Expedición actualizada: {Nombre}", dto.Nombre);
    }

    public async Task EliminarExpedicion(Guid id)
    {
        Expedicion expedicion = await ObtenerPorId(id);
        
        _unitOfWork.Expediciones.Eliminar(expedicion);
        await _unitOfWork.SaveChangesAsync();
        
        _logger.LogInformation("Expedición eliminada lógicamente: {Id}", id);
    }

    #region MetodosPrivados

    private async Task<Expedicion> ObtenerPorId(Guid id)
    {
        Expedicion? expedicion = await _unitOfWork.Expediciones.ObtenerPorIdAsync(id);

        if (expedicion == null)
        {
            _logger.LogWarning("Expedicion con el Id {Id} no éxiste", id);
            throw new NotFoundException($"Expedicion con el Id {id} no existe");
        }
        
        return expedicion;
    }

    #endregion
}
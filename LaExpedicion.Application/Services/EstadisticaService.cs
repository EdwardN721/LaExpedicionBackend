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

public class EstadisticaService : IEstadisticaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EstadisticaService> _logger;

    public EstadisticaService(IUnitOfWork unitOfWork, ILogger<EstadisticaService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<PagedList<EstadisticaDto>> ObtenerEstadisticas(ItemParameters itemParameters)
    {
        Expression<Func<Estadistica, bool>>? filter = null;

        var (registros, total) = await _unitOfWork.Estadisticas.ObtenerPaginadosAsync(filter, itemParameters.PageNumber, itemParameters.PageSize);
        List<EstadisticaDto> items = registros.Select(item => item.MapToDto()).ToList();
        
        return new PagedList<EstadisticaDto>(
            items, total, itemParameters.PageNumber, itemParameters.PageSize);
    }

    public async Task<EstadisticaDto> ObtenerEstadisticasPersonaje(Guid idPersonaje)
    {
        Estadistica estadistica = await ObtenerPorId(idPersonaje);
        return estadistica.MapToDto();
    }

    public async Task<EstadisticaDto> AgregarEstadisticas(CrearEstadisticaDto dto)
    {
        Estadistica estadistica = dto.MapToEntity(); 
        await _unitOfWork.Estadisticas.AgregarAsync(estadistica);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Estadistica agregada: {Id}", estadistica.Id);
        return estadistica.MapToDto();
    }

    public async Task ActualizarEstadisticas(Guid id, ActualizarEstadisticaDto dto)
    {
        Estadistica estadistica = await ObtenerPorId(id);
        estadistica.UpdateEntity(dto);
        _unitOfWork.Estadisticas.Actualizar(estadistica);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Estadistica actualizada: {Id}", estadistica.Id);
    }

    public async Task EliminarEstadistica(Guid id)
    {
        Estadistica estadistica = await ObtenerPorId(id);
        _unitOfWork.Estadisticas.Eliminar(estadistica);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogWarning("Estadistica eliminada: {Id}", estadistica.Id);
    }

    #region MetodosPrivados

    private async Task<Estadistica> ObtenerPorId(Guid id)
    {
        Estadistica? estadistica = await _unitOfWork.Estadisticas.ObtenerPorIdAsync(id);
        if (estadistica == null)
        {
            _logger.LogWarning("Estadistica no encontrada: {Id}", id);
            throw new NotFoundException($"Estadistica no encontrada por su Id {id}");
        }
        return estadistica;
    }

    #endregion
}
using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Interfaces;
using LaExpedicion.Application.Mappers;
using LaExpedicion.Domain.Entities;
using LaExpedicion.Domain.Exceptions;
using LaExpedicion.Domain.Interfaces;
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

    public async Task<IEnumerable<EstadisticaDto>> ObtenerEstadisticas()
    {
        List<Estadistica> estadisticas = (await _unitOfWork.Estadisticas.ObtenerTodosAsync()).ToList();
        _logger.LogInformation("EstadisticasTotales: {Contador}", estadisticas.Count);
        return estadisticas.MapToDto();
    }

    public async Task<EstadisticaDto> ObtenerEstadisticasPersonaje(Guid idPersonaje)
    {
        Estadistica estadistica = await ObtenerPorId(idPersonaje);
        return estadistica.MapToDto();
    }

    public async Task<EstadisticaDto> AgregarEstadisticas(CrearEstadisticaDto dto)
    {
        Estadistica estadistica = dto.MapToEntity(); // Hacerla random
        await _unitOfWork.Estadisticas.AgregarAsync(estadistica);
        _logger.LogInformation("Estadistica agregada: {Id}", estadistica.Id);
        return estadistica.MapToDto();
    }

    public async Task ActualizarEstadisticas(Guid id, ActualizarEstadisticaDto dto)
    {
        Estadistica estadistica = await ObtenerPorId(id);
        estadistica.UpdateEntity(dto);
        _unitOfWork.Estadisticas.Actualizar(estadistica);
        _logger.LogInformation("Estadistica actualizada: {Id}", estadistica.Id);
    }

    public async Task EliminarEstadistica(Guid id)
    {
        Estadistica estadistica = await ObtenerPorId(id);
        _unitOfWork.Estadisticas.Eliminar(estadistica);
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
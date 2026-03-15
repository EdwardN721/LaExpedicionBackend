using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Interfaces;
using LaExpedicion.Application.Mappers;
using LaExpedicion.Domain.Entities;
using LaExpedicion.Domain.Exceptions;
using LaExpedicion.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace LaExpedicion.Application.Services;

public class EtiquetaService : IEtiquetaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EtiquetaService> _logger;

    public EtiquetaService(IUnitOfWork unitOfWork, ILogger<EtiquetaService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<EtiquetaDto>> ObtenerEtiquetas()
    {
        List<Etiqueta> etiquetas = (await _unitOfWork.Etiquetas.ObtenerTodosAsync()).ToList();
        _logger.LogInformation("ObtenerEtiquetas: {Cantidad}", etiquetas.Count);
        return etiquetas.MapToDto();
    }

    public async Task<EtiquetaDto> ObtenerEtiquetaPorId(Guid id)
    {
        Etiqueta etiqueta = await ObtenerPorId(id);
        return etiqueta.MapToDto();
    }

    public async Task<Guid> ObtenerEtiquetaPorNombre(string nombre)
    {
        _logger.LogInformation("Buscando etiqueta: {Nombre}", nombre);
        
        Etiqueta? etiqueta = await _unitOfWork.Etiquetas.GetFirstOrDefaultAsync(e => e.Nombre == nombre);
        
        if (etiqueta == null)
        {
            _logger.LogWarning("La etiqueta {Nombre} no fue encontrada en la base de datos.", nombre);
            throw new KeyNotFoundException($"No se encontró una etiqueta con el nombre '{nombre}'.");
        }

        return etiqueta.Id;
    }

    public async Task<EtiquetaDto> CrearEtiqueta(CrearEtiquetaDto dto)
    {
        Etiqueta nuevaEtiqueta = dto.MapToEntity();
        await _unitOfWork.Etiquetas.AgregarAsync(nuevaEtiqueta);
        _logger.LogInformation("Etiqueta creada: {Nombre}", dto.Nombre);
        return nuevaEtiqueta.MapToDto();
    }

    public async Task ActualizarEtiqueta(Guid id, ActualizarEtiquetaDto dto)
    {
        Etiqueta etiqueta = await ObtenerPorId(id);
        etiqueta.UpdateEntity(dto);
        _unitOfWork.Etiquetas.Actualizar(etiqueta);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Etiqueta actualizada: {Nombre}", dto.Nombre);
    }

    public async Task EliminarEtiqueta(Guid id)
    {
        Etiqueta etiqueta = await ObtenerPorId(id);
        _unitOfWork.Etiquetas.Eliminar(etiqueta);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogWarning("Etiqueta eliminada: {NombreEtiqueta}", etiqueta.Nombre);
    }

    #region MetodosPrivados

    private async Task<Etiqueta> ObtenerPorId(Guid id)
    {
        Etiqueta? etiqueta = await _unitOfWork.Etiquetas.ObtenerPorIdAsync(id);
        return etiqueta ?? throw new NotFoundException($"No se encontro la etiqueta con el Id: {id}");
    }

    #endregion

}
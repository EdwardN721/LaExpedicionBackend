using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Interfaces;
using LaExpedicion.Application.Mappers;
using LaExpedicion.Domain.Entities;
using LaExpedicion.Domain.Exceptions;
using LaExpedicion.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace LaExpedicion.Application.Services;

public class PersonajeService : IPersonajeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PersonajeService> _logger;

    public PersonajeService(IUnitOfWork unitOfWork, ILogger<PersonajeService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<PersonajeDto>> ObtenerTodosPersonajes()
    {
        List<Personaje> personajes = (await _unitOfWork.Personajes.ObtenerTodosAsync()).ToList();
        _logger.LogInformation("Obtener Personajes con Total: {Cantidad}", personajes.Count);
        return personajes.MapToDto();
    }

    public async Task<PersonajeDto> ObtenerPersonajePorId(Guid id)
    {
        Personaje personaje = await ObtenerPorId(id);
        return personaje.MapToDto();
    }

    public async Task<PersonajeDto> CrearPersonaje(CrearPersonajeDto dto)
    {
      await _unitOfWork.BeginTransactionAsync();
      _logger.LogInformation("Creando personaje Usuario: {NombreUsuario} - {IdUsuario}.", dto.NombreUsuario, dto.UsuarioId);
      try
      {
          Personaje nuevoPersonaje = dto.MapToEntity();
          await _unitOfWork.Personajes.AgregarAsync(nuevoPersonaje);
          _logger.LogInformation("Personaje creado {NombreUsuario}", dto.NombreUsuario);
          // Asignar estadisticas
          CrearEstadisticaDto crearEstadisticaDto = new CrearEstadisticaDto
          {
              PersonajeId = nuevoPersonaje.Id,
          };

          Estadistica nuevasEstadisticas = crearEstadisticaDto.MapToEntity(); 
          await _unitOfWork.Estadisticas.AgregarAsync(nuevasEstadisticas);
          
          _logger.LogInformation("Estadistica agregada {NombreUsuario}", dto.NombreUsuario);
          return nuevoPersonaje.MapToDto();
      } catch (Exception e)
      {
          _logger.LogError(e, "Error creando personaje");
          await _unitOfWork.RollbackTransactionAsync();
          throw;
      }
    }

    public async Task ActualizarPersonaje(Guid id, ActualizarPersonajeDto dto)
    {
        Personaje personaje = await ObtenerPorId(id);
        personaje.UpdateEntity(dto);
        _logger.LogInformation("Actualizando personaje {NombreUsuario}", dto.NombreUsuario);
    }

    public async Task EliminarPersonaje(Guid id)
    {
        Personaje personaje = await ObtenerPorId(id);
        _logger.LogWarning("Personaje eliminado: {NombreUsuario}", personaje.UsuarioId);
    }

    #region MetodosPrivados

    private async Task<Personaje> ObtenerPorId(Guid id)
    {
        Personaje? personaje = await _unitOfWork.Personajes.ObtenerPorIdAsync(id);

        if (personaje == null)
        {
            _logger.LogWarning("No existe personaje con id: {Id}", id);
            throw new NotFoundException($"No existe personaje con id: {id}");
        }
        
        return personaje;
    }

    #endregion
}
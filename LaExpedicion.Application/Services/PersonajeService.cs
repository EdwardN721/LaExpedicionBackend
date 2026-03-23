using System.Linq.Expressions;
using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Interfaces;
using LaExpedicion.Application.Mappers;
using LaExpedicion.Application.Parameters;
using LaExpedicion.Domain.Entities;
using LaExpedicion.Domain.Enum;
using LaExpedicion.Domain.Exceptions;
using LaExpedicion.Domain.Interfaces;
using LaExpedicion.Shared.Extensions;
using LaExpedicion.Shared.Pagination;
using Microsoft.Extensions.Logging;

namespace LaExpedicion.Application.Services;

public class PersonajeService : IPersonajeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEtiquetaService _etiquetaService;
    private readonly ILogger<PersonajeService> _logger;

    public PersonajeService(IUnitOfWork unitOfWork, ILogger<PersonajeService> logger,  IEtiquetaService etiquetaService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _etiquetaService = etiquetaService ?? throw new ArgumentNullException(nameof(etiquetaService));
    }

    public async Task<PagedList<PersonajeDto>> ObtenerTodosPersonajes(ItemParameters parameters)
    {
        Expression<Func<Personaje, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(parameters.Nombre))
        {
            filter = x => x.NombreUsuario.Contains(parameters.Nombre);
        }
        
        var (registros, total) = await _unitOfWork.Personajes.ObtenerPaginadosAsync(
            filter,
            parameters.PageNumber,
            parameters.PageSize,
            x => x.Etiqueta!,
            x => x.Estadistica!
            );
        
        List<PersonajeDto> personajes = registros.Select(personaje => personaje.MapToDto()).ToList();
        
        _logger.LogInformation("Total de registros: {PersonajesCount}", personajes.Count);
        return new PagedList<PersonajeDto>(personajes, total, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<PersonajeDto> ObtenerPersonajePorId(Guid id)
    {
        Personaje personaje = await ObtenerPorId(id);
        return personaje.MapToDto();
    }

    public async Task<PersonajeDto> ObtenerPersonajePorUsuarioId(Guid usuarioId)
    {
        Personaje peronsaje = await ObtenerPorId(usuarioId);
        return peronsaje.MapToDto();
    }

    public async Task<PersonajeDto> CrearPersonaje(CrearPersonajeDto dto)
    {
      await _unitOfWork.BeginTransactionAsync();
      _logger.LogInformation("Creando personaje Usuario: {NombreUsuario} - {IdUsuario}.", dto.NombreUsuario, dto.UsuarioId);
      try
      {
          Personaje nuevoPersonaje = dto.MapToEntity(); // Se crea sin etiqueta

          // Genera estadisticas
          (EstadisticasDto estadisticas, int promedio) = GenerarEstadisticas();
          
          Guid etiquetaAsignada = Guid.Empty; 
          if (promedio <= 3)
              etiquetaAsignada = await _etiquetaService.ObtenerEtiquetaPorNombre(EnumEtiquetas.Novato.GetEnumDescription());
          else if (promedio <= 6)
              etiquetaAsignada = await _etiquetaService.ObtenerEtiquetaPorNombre(EnumEtiquetas.Aventurero.GetEnumDescription());
          else if  (promedio <= 9)
              etiquetaAsignada = await _etiquetaService.ObtenerEtiquetaPorNombre(EnumEtiquetas.TalentoNato.GetEnumDescription());
          else if (promedio == 10)
              etiquetaAsignada = await _etiquetaService.ObtenerEtiquetaPorNombre(EnumEtiquetas.Genio.GetEnumDescription());

          nuevoPersonaje.EtiquetaId = etiquetaAsignada; // Pasamos etiqueta
          
          // AGREGAR Y GUARDAR EL PERSONAJE PRIMERO
          await _unitOfWork.Personajes.AgregarAsync(nuevoPersonaje);
          await _unitOfWork.SaveChangesAsync(); 
          _logger.LogInformation("Personaje guardado en BD temporalmente {NombreUsuario}", dto.NombreUsuario);
          
          CrearEstadisticaDto crearEstadisticaDto = new CrearEstadisticaDto
          {
              PersonajeId = nuevoPersonaje.Id,
              Energia = estadisticas.Energia,
              Magia = estadisticas.Magia,
              Fuerza = estadisticas.Fuerza,
              Salud = estadisticas.Salud,
              Mana = estadisticas.Mana,
          }; // Objeto para setterar estadisticas
          
          Estadistica nuevasEstadisticas = crearEstadisticaDto.MapToEntity();
          await _unitOfWork.Estadisticas.AgregarAsync(nuevasEstadisticas);
          await  _unitOfWork.SaveChangesAsync();

          nuevoPersonaje.Estadistica = nuevasEstadisticas;
          nuevoPersonaje.Etiqueta = await _unitOfWork.Etiquetas.GetFirstOrDefaultAsync(x => x.Id == etiquetaAsignada);
          
          await _unitOfWork.SaveChangesAsync();
          await _unitOfWork.CommitTransactionAsync();
          
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
        
        _unitOfWork.Personajes.Actualizar(personaje);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Actualizando personaje {NombreUsuario}", dto.NombreUsuario);
    }

    public async Task EliminarPersonaje(Guid id)
    {
        Personaje personaje = await ObtenerPorId(id);
        
        _unitOfWork.Personajes.Eliminar(personaje);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogWarning("Personaje eliminado: {NombreUsuario}", personaje.UsuarioId);
    }

    #region MetodosPrivados

    private async Task<Personaje> ObtenerPorId(Guid id)
    {
        Personaje? personaje = await _unitOfWork.Personajes.GetFirstOrDefaultAsync(
            p => p.Id == id,
            p => p.Etiqueta!, // Join
            p => p.Estadistica! // Join
        );
        
        if (personaje == null)
        {
            _logger.LogWarning("No existe personaje con id: {Id}", id);
            throw new NotFoundException($"No existe personaje con id: {id}");
        }
        
        return personaje;
    }

    private (EstadisticasDto, int promedio) GenerarEstadisticas()
    {
        int fuerza = Random.Shared.Next(0, 11);
        int energia = Random.Shared.Next(0, 11);
        int magia = Random.Shared.Next(0, 11);
        int mana = Random.Shared.Next(0, 11);
        int salud = Random.Shared.Next(50, 100);

        int avg = (fuerza + energia + magia + mana) / 4;
        
        return (new EstadisticasDto
        {
            Salud = salud,
            Energia = energia,
            Magia = magia,
            Mana = mana,
            Fuerza = fuerza,
        },  avg);
    }
    
    #endregion
}
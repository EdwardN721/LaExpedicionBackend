using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;

namespace LaExpedicion.Application.Interfaces;

public interface IEstadisticaService
{
    Task<IEnumerable<EstadisticaDto>> ObtenerEstadisticas();
    Task<EstadisticaDto> ObtenerEstadisticasPersonaje(Guid idPersonaje);
    Task<EstadisticaDto> AgregarEstadisticas(CrearEstadisticaDto dto);
    Task ActualizarEstadisticas(Guid id, ActualizarEstadisticaDto dto);
    Task EliminarEstadistica(Guid id);
}
using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Parameters;
using LaExpedicion.Shared.Pagination;

namespace LaExpedicion.Application.Interfaces;

public interface IEstadisticaService
{
    Task<PagedList<EstadisticaDto>> ObtenerEstadisticas(ItemParameters itemParameters);
    Task<EstadisticaDto> ObtenerEstadisticasPersonaje(Guid idPersonaje);
    Task<EstadisticaDto> AgregarEstadisticas(CrearEstadisticaDto dto);
    Task ActualizarEstadisticas(Guid id, ActualizarEstadisticaDto dto);
    Task EliminarEstadistica(Guid id);
}
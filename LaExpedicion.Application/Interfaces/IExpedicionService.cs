using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Parameters;
using LaExpedicion.Shared.Pagination;

namespace LaExpedicion.Application.Interfaces;

public interface IExpedicionService
{
    Task<PagedList<ExpedicionDto>> ObtenerTodasExpediciones(ExpedicionParameters parameters);
    Task<ExpedicionDto> ObtenerExpedicionPorId(Guid id);
    Task<ExpedicionDto> CrearExpedicion(CrearExpedicionDto dto);
    Task ActualizarExpedicion(Guid id, ActualizarExpedicionDto dto);
    Task EliminarExpedicion(Guid id);
}
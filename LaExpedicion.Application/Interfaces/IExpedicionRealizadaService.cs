using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Parameters;
using LaExpedicion.Shared.Pagination;

namespace LaExpedicion.Application.Interfaces;

public interface IExpedicionRealizadaService
{
    /// <summary>
    /// Historial de aventuras de un personaje
    /// </summary>
    /// <param name="personajeId">Id del Personaje</param>
    /// <param name="parameters">Parametros paginado</param>
    /// <returns>Expediciones realizadas</returns>
    Task<PagedList<ExpedicionRealizadaDto>> ObtenerHistorialDePersonaje(Guid personajeId, RequestParameters parameters);
    
    /// <summary>
    /// El personaje comieza con la expedicion
    /// </summary>
    /// <param name="personajeId">Id del personaje</param>
    /// <param name="expedicionId">Id de la expedicion</param>
    /// <returns>Expedicion actual</returns>
    Task<ExpedicionRealizadaDto> EmprenderExpedicion(Guid personajeId, Guid expedicionId);
}
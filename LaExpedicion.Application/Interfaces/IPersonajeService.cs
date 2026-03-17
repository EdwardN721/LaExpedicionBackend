using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Parameters;
using LaExpedicion.Shared.Pagination;

namespace LaExpedicion.Application.Interfaces;

public interface IPersonajeService
{
    Task<PagedList<PersonajeDto>> ObtenerTodosPersonajes(ItemParameters Parameters);
    Task<PersonajeDto> ObtenerPersonajePorId(Guid id);
    Task<PersonajeDto> CrearPersonaje(CrearPersonajeDto dto);
    Task ActualizarPersonaje(Guid id, ActualizarPersonajeDto dto);
    Task EliminarPersonaje(Guid id);
}
using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;

namespace LaExpedicion.Application.Interfaces;

public interface IPersonajeService
{
    Task<IEnumerable<PersonajeDto>> ObtenerTodosPersonajes();
    Task<PersonajeDto> ObtenerPersonajePorId(Guid id);
    Task<PersonajeDto> CrearPersonaje(CrearPersonajeDto dto);
    Task ActualizarPersonaje(Guid id, ActualizarPersonajeDto dto);
    Task EliminarPersonaje(Guid id);
}
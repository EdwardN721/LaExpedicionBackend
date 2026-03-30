using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Domain.Entities;

namespace LaExpedicion.Application.Mappers;

public static class PersonajeMapper
{
    public static Personaje MapToEntity(this CrearPersonajeDto dto)
    {
        return new Personaje
        {
            NombreUsuario = dto.NombreUsuario,
            UsuarioId = dto.UsuarioId,
            Nivel = 1,
            Experiencia = 0,
            Dinero = 0.0,
            SaludActual = 10
        };
    }

    public static void UpdateEntity(this Personaje personaje, ActualizarPersonajeDto dto)
    {
        personaje.NombreUsuario = dto.NombreUsuario;
        personaje.EtiquetaId = dto.EtiquetaId;
    }

    public static PersonajeDto MapToDto(this Personaje personaje)
    {
        return new PersonajeDto
        {
            Id = personaje.Id,
            UsuarioId = personaje.UsuarioId,
            NombreUsuario = personaje.NombreUsuario,
            Nivel = personaje.Nivel,
            Experiencia = personaje.Experiencia,
            Dinero = personaje.Dinero,
            SaludActual = personaje.SaludActual,
            Etiqueta = personaje.Etiqueta?.Nombre ?? "N/A",
            Salud = personaje.Estadistica?.Salud ?? 0,
            Energia = personaje.Estadistica?.Energia ?? 0,
            Fuerza = personaje.Estadistica?.Fuerza ?? 0,
            Mana = personaje.Estadistica?.Mana ?? 0,
            Magia = personaje.Estadistica?.Magia ?? 0,
        };
    }

    public static IEnumerable<PersonajeDto> MapToDto(this IEnumerable<Personaje>? personajes)
    {
        if (personajes == null) return Enumerable.Empty<PersonajeDto>();
        return personajes.Select(MapToDto);
    }
}
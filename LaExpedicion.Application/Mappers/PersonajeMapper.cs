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
            EtiquetaId = dto.EtiquetaId,
            UsuarioId = dto.UsuarioId
        };
    }

    public static PersonajeDto MapToDto(this Personaje personaje)
    {
        return new PersonajeDto
        {
            NombreUsuario = personaje.NombreUsuario,
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
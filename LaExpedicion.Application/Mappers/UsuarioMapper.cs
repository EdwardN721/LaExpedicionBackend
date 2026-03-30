using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Domain.Entities;

namespace LaExpedicion.Application.Mappers;

public static class UsuarioMapper
{
    public static Usuario MapToEntity(this CrearUsuarioDto dto)
    {
        return new Usuario
        {
            Nombre = dto.Nombre,
            PrimerApellido = dto.PrimerApellido,
            SegundoApellido = dto.SegundoApellido ?? string.Empty,
            PhoneNumber = dto.Telefono,
            Email = dto.Correo,
            UserName = dto.Correo // En Identity, usualmente el UserName es el mismo correo
        };
    }

    public static void UpdateEntity(this Usuario entity, ActualizarUsuarioDto dto)
    {
        entity.Nombre = dto.Nombre;
        entity.PrimerApellido = dto.PrimerApellido;
        entity.SegundoApellido = dto.SegundoApellido ?? "";
        entity.PhoneNumber = dto.Telefono;
    }

    public static UsuarioDto MapToDto(this Usuario entity)
    {
        return new UsuarioDto
        {
            Id = entity.Id,
            Nombre = entity.Nombre,
            PrimerApellido = entity.PrimerApellido,
            SegundoApellido = entity.SegundoApellido,
            Email = entity.Email ?? string.Empty,
            UserName = entity.UserName ?? string.Empty
        };
    }

    public static IEnumerable<UsuarioDto> MapToDto(this List<Usuario>? entities)
    {
        return entities?.Select(MapToDto) ?? Enumerable.Empty<UsuarioDto>();
    }
}
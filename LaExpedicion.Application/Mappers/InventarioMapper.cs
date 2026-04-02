using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Domain.Entities;

namespace LaExpedicion.Application.Mappers;

public static class InventarioMapper
{
    public static Inventario MapToEntity(this CrearInventarioDto dto)
    {
        return new Inventario
        {
            UsosRestantes = dto.UsosRestantes,
            PersonajeId = dto.PersonajeId,
            ItemId = dto.ItemId,
            Equipado = dto.Equipado,
        };
    }

    public static void UpdateEntity(this Inventario inventario, ActualizarInventarioDto dto)
    {
        inventario.UsosRestantes = dto.UsosRestantes;
        inventario.Equipado = dto.Equipado;
    } 

    public static InventarioDto MapToDto(this Inventario inventario)
    {
        return new InventarioDto
        {
            Id = inventario.Id,
            ItemId = inventario.ItemId,
            NombrePersonaje = inventario.Personaje?.NombreUsuario ?? "Desconcido.",
            NombreItem = inventario.Item?.Nombre ?? "Desconcido.",
            Equipado = inventario.Equipado,
            UsosRestantes = inventario.UsosRestantes,
        };
    }

    public static IEnumerable<InventarioDto> MapToDto(this IEnumerable<Inventario>? inventarios)
    {
        return inventarios?.Select(MapToDto) ?? Enumerable.Empty<InventarioDto>();
    }
}
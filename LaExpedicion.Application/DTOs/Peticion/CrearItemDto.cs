using Microsoft.AspNetCore.Http;

namespace LaExpedicion.Application.DTOs.Peticion;

public record CrearItemDto
{
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; } = string.Empty;
    public double Precio { get; init; }
    public IFormFile? Imagen { get; init; }
    public IEnumerable<CrearItemModificadorDto>? Modificadores { get; init; }
};
using Microsoft.AspNetCore.Http;

namespace LaExpedicion.Application.DTOs.Peticion;

public record CrearItemDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; } = string.Empty;
    public double Precio { get; set; }
    public IFormFile? Imagen { get; set; }
    public int TipoItem { get; set; }
    public IEnumerable<CrearItemModificadorDto>? Modificadores { get; set; }
};
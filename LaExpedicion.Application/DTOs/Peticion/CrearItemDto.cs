namespace LaExpedicion.Application.DTOs.Peticion;

public record CrearItemDto
{
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; } = string.Empty;
    
    public IEnumerable<CrearItemModificadorDto>? Modificadores { get; init; }
};
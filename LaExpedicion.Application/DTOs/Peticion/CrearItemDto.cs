namespace LaExpedicion.Application.DTOs.Peticion;

public record CrearItemDto
{
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; } = string.Empty;
    public double Precio { get; set; }
    
    public IEnumerable<CrearItemModificadorDto>? Modificadores { get; init; }
};
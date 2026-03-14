namespace LaExpedicion.Application.DTOs.Peticion;

public record CrearEtiquetaDto
{
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; } = string.Empty;
};
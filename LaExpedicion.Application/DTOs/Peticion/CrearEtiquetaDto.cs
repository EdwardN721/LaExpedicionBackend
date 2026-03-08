namespace LaExpedicion.Application.DTOs.Peticion;

public record CrearEtiquetaDto
{
    private string Nombre { get; init; } = string.Empty;
    private string? Descripcion { get; init; } = string.Empty;
};
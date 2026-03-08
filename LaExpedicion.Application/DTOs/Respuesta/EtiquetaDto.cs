namespace LaExpedicion.Application.DTOs.Respuesta;

public record EtiquetaDto
{
    private string Nombre { get; init; } = string.Empty;
    private string? Descripcion { get; init; } = string.Empty;
};
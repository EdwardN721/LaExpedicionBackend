namespace LaExpedicion.Application.DTOs.Respuesta;

public record EtiquetaDto
{
    public Guid Id { get; set; }
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; } = string.Empty;
};
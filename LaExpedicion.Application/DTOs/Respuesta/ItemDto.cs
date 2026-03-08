namespace LaExpedicion.Application.DTOs.Respuesta;

public record ItemDto
{
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; } = string.Empty;
    public string? EstadisticaAfectada { get; init; } = string.Empty;
    public int? ValorAjuste { get; init; }
    
};
namespace LaExpedicion.Application.DTOs.Respuesta;

public record InventarioDto
{
    public string? NombrePersonaje { get; init; } = string.Empty;
    public string? NombreItem { get; init; } = string.Empty;
    public bool Equipado { get; init; }
    public int UsosRestantes { get; init; }
};
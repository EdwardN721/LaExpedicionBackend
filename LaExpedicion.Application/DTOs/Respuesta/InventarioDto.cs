namespace LaExpedicion.Application.DTOs.Respuesta;

public record InventarioDto
{
    public Guid Id { get; init; } 
    public Guid ItemId { get; init; }
    public string? NombrePersonaje { get; init; } = string.Empty;
    public string? NombreItem { get; init; } = string.Empty;
    public bool Equipado { get; init; }
    public int UsosRestantes { get; init; }
};
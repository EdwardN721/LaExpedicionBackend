namespace LaExpedicion.Application.DTOs.Respuesta;

public record PersonajeDto
{
    public string NombreUsuario { get; init; } = string.Empty;
    public string? Etiqueta { get; init; } = string.Empty;
    public int? Fuerza { get; init; }
    public int? Energia { get; init; }
    public int? Magia { get; init; }
    public int? Mana { get; init; }
    public int? Salud { get; init; }
};
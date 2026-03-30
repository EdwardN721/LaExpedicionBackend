namespace LaExpedicion.Application.DTOs.Respuesta;

public record PersonajeDto
{
    public Guid Id { get; init; }
    public Guid UsuarioId { get; init; }
    public string NombreUsuario { get; init; } = string.Empty;
    public int Nivel { get; set; }
    public int Experiencia { get; set; }
    public double Dinero { get; set; }
    public int SaludActual { get; set; }
    public string? Etiqueta { get; init; } = string.Empty;
    public int? Fuerza { get; init; }
    public int? Energia { get; init; }
    public int? Magia { get; init; }
    public int? Mana { get; init; }
    public int? Salud { get; init; }
};
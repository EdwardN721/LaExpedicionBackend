namespace LaExpedicion.Application.DTOs.Respuesta;

public record EstadisticaDto
{
    public Guid Id { get; init; }
    public Guid PersonajeId { get; init; }
    public int Salud { get; init; }
    public int Fuerza { get; init; }
    public int Energia { get; init; }
    public int Magia { get; init; }
    public int Mana { get; init; }
};
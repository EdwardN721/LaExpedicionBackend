namespace LaExpedicion.Application.DTOs.Peticion;

public record CrearEstadisticaDto
{
    private Guid PersonajeId { get; init; }
    private int Salud { get; init; }
    private int Fuerza { get; init; }
    private int Energia { get; init; }
    private int Magia { get; init; }
    private int Mana { get; init; }
};
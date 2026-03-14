namespace LaExpedicion.Application.DTOs.Peticion;

public record EstadisticasDto
{
    public int Salud { get; init; }
    public int Fuerza { get; init; }
    public int Energia { get; init; }
    public int Magia { get; init; }
    public int Mana { get; init; }
};
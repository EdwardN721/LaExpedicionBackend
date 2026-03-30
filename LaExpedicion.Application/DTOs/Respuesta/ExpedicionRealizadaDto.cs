namespace LaExpedicion.Application.DTOs.Respuesta;

public record ExpedicionRealizadaDto
{
    public Guid Id { get; init; }
    public Guid PersonajeId { get; init; }
    public Guid ExpedicionId { get; init; }
    public string Nombre { get; init; } = string.Empty; 
    public DateTime FechaInicio { get; init; }
    public DateTime? FechaFin { get; init; }
    public string Resultado { get; init; } = string.Empty;
    public int ExperienciaGanada { get; init; }
    public double DineroGanado { get; init; }
};
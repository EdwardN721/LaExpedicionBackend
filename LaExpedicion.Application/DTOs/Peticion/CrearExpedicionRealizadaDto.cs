using LaExpedicion.Domain.Enum;

namespace LaExpedicion.Application.DTOs.Peticion;

public record CrearExpedicionRealizadaDto
{
    private Guid PersonajeId { get; init; }
    private Guid ExpedicionId { get; init; }
    private DateTime FechaInicio { get; init; }
    private DateTime? FechaFin { get; init; }
    private EnumResultado Resultado { get; init; }
    private int ExperienciaGanada { get; init; }
    private double DineroGanado { get; init; }
};
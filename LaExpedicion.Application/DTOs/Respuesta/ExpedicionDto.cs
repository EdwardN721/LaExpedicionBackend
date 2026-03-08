namespace LaExpedicion.Application.DTOs.Respuesta;

public record ExpedicionDto
{
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; } = string.Empty;
    public int Experiencia { get; init; }
    public double Dinero { get; init; }
};

public record ExpedicionRealziadaDto
{
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; } = string.Empty;
    public string? NombrePersonaje { get; init; } = string.Empty;
    public DateTime FechaInicio { get; init; }
    public DateTime? FechaFin { get; init; }
    public string? Resultado { get; init; }
    public int ExperienciaGanada { get; init; }
    public double DineroGanado { get; init; }
};
namespace LaExpedicion.Application.DTOs.Respuesta;

public record OpcionEventoDto
{
    public string IdOpcion { get; init; } = string.Empty;
    public string Texto { get; init; } = string.Empty;
    public string EstadisticaRequerida { get; init; } = string.Empty;
    public int Dificultad { get; init; }
    public string TextoExito { get; init; } = string.Empty;
    public string TextoFallo { get; init; } = string.Empty;
    public int RecompensaOro { get; init; }
    public int RecompensaXp { get; init; }
    public int PenalizacionSalud { get; init; }
    public string ItemRecompensa { get; init; } = string.Empty;
};
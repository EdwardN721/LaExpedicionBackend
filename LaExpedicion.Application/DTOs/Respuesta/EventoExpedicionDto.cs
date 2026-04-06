namespace LaExpedicion.Application.DTOs.Respuesta;

public record EventoExpedicionDto
{
    public string Id { get; init; } = string.Empty;
    public string Nombre { get; init; } = string.Empty;
    public string Narrativa { get; init; } = string.Empty;
    public List<OpcionEventoDto> Opciones { get; init; } = new();
};
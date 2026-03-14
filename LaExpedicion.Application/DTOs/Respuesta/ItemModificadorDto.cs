namespace LaExpedicion.Application.DTOs.Respuesta;

public record ItemModificadorDto
{
    public Guid Id { get; init; }
    public Guid ItemId { get; init; }
    public string EstadisticaAfectada { get; init; } = string.Empty;
    public int ValorAjuste { get; init; }
};
namespace LaExpedicion.Application.DTOs.Respuesta;

public record ItemDto
{
    public Guid Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; } = string.Empty;
    public double Precio { get; set; }
    public string? EstadisticaAfectada { get; init; } = string.Empty;
    public int? ValorAjuste { get; init; }
    public IEnumerable<ItemModificadorDto> ItemModificador { get; init; } = new List<ItemModificadorDto>();
};
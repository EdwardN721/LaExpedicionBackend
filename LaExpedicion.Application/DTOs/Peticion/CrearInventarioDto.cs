namespace LaExpedicion.Application.DTOs.Peticion;

public record CrearInventarioDto
{
    public Guid PersonajeId { get; init; }
    public Guid ItemId { get; init; }
    public bool Equipado { get; init; }
    public int UsosRestantes { get; init; }
};
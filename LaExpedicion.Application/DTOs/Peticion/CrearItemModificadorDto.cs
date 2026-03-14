using LaExpedicion.Domain.Enum;

namespace LaExpedicion.Application.DTOs.Peticion;

public record CrearItemModificadorDto
{
    public Guid ItemId { get; init; }
    public EnumEstadistica EstadisticaAfectada { get; init; }
    public int ValorAjustado { get; init; }
};
using LaExpedicion.Domain.Enum;

namespace LaExpedicion.Application.DTOs.Peticion;

public record CrearItemModificadorDto
{
    private Guid ItemId { get; init; }
    private EnumEstadistica EstadisticaAfectada { get; init; }
    private int ValorAjustado { get; init; }
};
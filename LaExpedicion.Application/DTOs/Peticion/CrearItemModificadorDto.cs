using LaExpedicion.Domain.Enum;

namespace LaExpedicion.Application.DTOs.Peticion;

public record CrearItemModificadorDto
{
    public EnumEstadistica EstadisticaAfectada { get; init; }
    public int ValorAjustado { get; init; }
};
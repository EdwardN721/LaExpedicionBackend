using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Domain.Entities;

namespace LaExpedicion.Application.Mappers;

public static class ExpedicionRealizadaMapper
{
    public static ExpedicionRealizada MapToEntity(this CrearExpedicionRealizadaDto dto)
    {
        return new ExpedicionRealizada
        {
            PersonajeId = dto.PersonajeId,
            ExpedicionId = dto.ExpedicionId,
            // Resultado, Dinero y Experiencia se calculan en el Service, no se confía en el DTO de entrada.
        };
    }

    public static ExpedicionRealizadaDto MapToDto(this ExpedicionRealizada entity)
    {
        return new ExpedicionRealizadaDto
        {
            Id = entity.Id,
            PersonajeId = entity.PersonajeId,
            ExpedicionId = entity.ExpedicionId,
            FechaInicio = entity.FechaInicio,
            FechaFin = entity.FechaFin,
            Resultado = entity.Resultado.ToString(),
            ExperienciaGanada = entity.ExperienciaGanada,
            DineroGanado = entity.DineroGanado
        };
    }

    public static IEnumerable<ExpedicionRealizadaDto> MapToDto(this IEnumerable<ExpedicionRealizada> entities)
    {
        return entities?.Select(MapToDto) ?? Enumerable.Empty<ExpedicionRealizadaDto>();
    }
}
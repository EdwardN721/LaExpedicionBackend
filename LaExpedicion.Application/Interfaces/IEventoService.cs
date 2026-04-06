using LaExpedicion.Application.DTOs.Respuesta;

namespace LaExpedicion.Application.Interfaces;

public interface IEventoService
{
    Task<EventoExpedicionDto> ObtenerEventoAleatorioAsync();
}
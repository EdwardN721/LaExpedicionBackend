using System.Security.Claims;
using System.Text.Json;
using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Interfaces;
using LaExpedicion.Application.Parameters;
using LaExpedicion.Shared.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaExpedicion.API.Controllers;

/// <summary>
/// Controlador que administra las expediciones realizadas
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ExpedicionRealizadaController : ControllerBase
{
    private readonly IExpedicionRealizadaService _expedicionRealizadaService;
    private readonly IEventoService _eventoService;

    public ExpedicionRealizadaController(IExpedicionRealizadaService expedicionRealizadaService,
        IEventoService eventoService)
    {
        _expedicionRealizadaService = expedicionRealizadaService ??
                                      throw new ArgumentNullException(nameof(expedicionRealizadaService));
        _eventoService = eventoService ?? throw new ArgumentNullException(nameof(eventoService));
    }

    /// <summary>
    /// Obtiene el historial de expediciones de un personaje específico con paginación
    /// </summary>
    [HttpGet("{personajeId}/historial")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ObtenerHistorial(Guid personajeId, [FromQuery] ExpedicionParameters parameters)
    {
        PagedList<ExpedicionRealizadaDto> pagedResult =
            await _expedicionRealizadaService.ObtenerHistorialDePersonaje(personajeId, parameters);

        var metadataJson = JsonSerializer.Serialize(pagedResult.Metadata);

        Response.Headers.Append("Access-Control-Expose-Headers", "X-Pagination");

        Response.Headers.Append("X-Pagination", metadataJson);

        return Ok(pagedResult);
    }

    /// <summary>
    /// Inicia una aventura (Expedición). Calcula estadísticas, RNG, durabilidad de objetos y devuelve un resultado.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> EmprenderAventura([FromBody] CrearExpedicionRealizadaDto dto)
    {
        string? userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid usuarioId))
            return Unauthorized(new { message = "Token inválido." });
        
        // Llamamos al servicio que contiene toda la lógica de batalla y transacciones
        ExpedicionRealizadaDto resultado = await _expedicionRealizadaService.EmprenderExpedicion(dto, usuarioId);

        return Ok(resultado);
    }

    /// <summary>
    /// Obtiene un evento aleatorio
    /// </summary>
    /// <returns></returns>
    [HttpGet("evento-aleatorio")]
    public async Task<ActionResult> ObtenerEvento()
    {
        EventoExpedicionDto evento = await _eventoService.ObtenerEventoAleatorioAsync();
        return Ok(evento);
    }
}
using System.Text.Json;
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

    public ExpedicionRealizadaController(IExpedicionRealizadaService expedicionRealizadaService)
    {
        _expedicionRealizadaService = expedicionRealizadaService;
    }

    /// <summary>
    /// Obtiene el historial de expediciones de un personaje específico con paginación
    /// </summary>
    [HttpGet("{personajeId}/historial")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ObtenerHistorial(Guid personajeId, [FromQuery] RequestParameters parameters)
    {
        PagedList<ExpedicionRealizadaDto> pagedResult = await _expedicionRealizadaService.ObtenerHistorialDePersonaje(personajeId, parameters);
        
        string metadataJson = JsonSerializer.Serialize(pagedResult);
        
        // Agregamos la información de paginación en el encabezado (Header) de la respuesta
        Response.Headers.Append("Access-Control-Expose-Headers", "X-Pagination");
        Response.Headers.Append("X-Pagination", metadataJson);

        return Ok(pagedResult);
    }

    /// <summary>
    /// Inicia una aventura (Expedición). Calcula estadísticas, RNG, durabilidad de objetos y devuelve un resultado.
    /// </summary>
    [HttpPost("{personajeId}/{expedicionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> EmprenderAventura(Guid personajeId, Guid expedicionId)
    {
        // Llamamos al servicio que contiene toda la lógica de batalla y transacciones
        var resultado = await _expedicionRealizadaService.EmprenderExpedicion(personajeId, expedicionId);
        
        return Ok(resultado);
    }
}
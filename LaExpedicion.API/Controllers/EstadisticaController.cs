using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LaExpedicion.API.Controllers;

/// <summary>
/// Controlador para administrar las estadisticas de los personajes
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EstadisticaController : ControllerBase
{
    private readonly IEstadisticaService _estadisticaService;

    public EstadisticaController(IEstadisticaService estadisticaService)
    {
        _estadisticaService = estadisticaService ??  throw new ArgumentNullException(nameof(estadisticaService));
    }

    /// <summary>
    /// Obtener estadisticas de los personajes creados
    /// </summary>
    /// <returns>Listado de estadisticas</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EstadisticaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerTodasEstadisticas()
    {
        IEnumerable<EstadisticaDto> estadisticas = await _estadisticaService.ObtenerEstadisticas();
        return Ok(estadisticas);
    }
    
    /// <summary>
    /// Obtener estadisticas de un personaje
    /// </summary>
    /// <param name="id">Id del personaje</param>
    /// <returns>Estadisitcas de personaje</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(EstadisticaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ObtenerEstadisticaPorPersonaje([FromRoute] Guid id)
    {
        EstadisticaDto estadistica = await _estadisticaService.ObtenerEstadisticasPersonaje(id);
        return Ok(estadistica);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ActualizarEstadisticaPersonaje([FromRoute] Guid id,
        [FromBody] ActualizarEstadisticaDto dto)
    {
        await _estadisticaService.ActualizarEstadisticas(id, dto);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EliminarEstadisticaPersonaje([FromRoute] Guid id)
    {
        await _estadisticaService.EliminarEstadistica(id);
        return NoContent();
    }
} 
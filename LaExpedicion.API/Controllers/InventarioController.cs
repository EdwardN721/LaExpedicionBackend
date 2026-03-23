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
/// Controlador que administra el inventario
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class InventarioController : ControllerBase
{
    private readonly IInventarioService _service;

    public InventarioController(IInventarioService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Obtiene todos los objetos en la mochila de un personaje específico
    /// </summary>
    [HttpGet("personaje/{personajeId:guid}")]
    [ProducesResponseType(typeof(PagedList<InventarioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> ObtenerInventario([FromRoute] Guid personajeId, [FromQuery] InventarioParameters parameters)
    {
        PagedList<InventarioDto> pagedResult = await _service.ObtenerInventarioDelPersonaje(personajeId, parameters);

        var metadata = JsonSerializer.Serialize(pagedResult.Metadata);
        
        Response.Headers.Append("Access-Control-Expose-Headers", "X-Pagination");
        Response.Headers.Append("X-Pagination", metadata);
        
        return Ok(pagedResult);
    }

    
    [HttpPost("agregar")]
    [ProducesResponseType(typeof(InventarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AgregarItem([FromBody] CrearInventarioDto dto)
    {
        var inventario = await _service.AgregarItem(dto);
        return Ok(inventario);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> ActualizarInventario([FromRoute] Guid id, [FromBody] ActualizarInventarioDto dto)
    {
        await _service.ActualizarInventario(id, dto);
        return NoContent();
    }

    [HttpPost("{id:guid}/usar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UsarItem([FromRoute] Guid id, [FromQuery] int usos = 1)
    {
        // Mecánica de juego: Gastar pociones o durabilidad del arma
        await _service.UsarItem(id, usos);
        return Ok(new { Mensaje = $"Se gastaron {usos} usos del item." });
    }

    [HttpPatch("{id:guid}/equipar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> EquiparItem([FromRoute] Guid id)
    {
        await _service.EquiparItem(id);
        return Ok(new { Mensaje = "Estado de equipamiento actualizado." });
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> EliminarDelInventario([FromRoute] Guid id)
    {
        // El jugador decide tirar el objeto de su mochila
        await _service.EliminarInventario(id);
        return NoContent();
    }
}
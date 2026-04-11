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
/// Controlador que administra el inventario
/// </summary>
[ApiController]
[Authorize]
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
    [Authorize(Roles = "Player")]
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
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(InventarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AgregarItem([FromBody] CrearInventarioDto dto)
    {
        var inventario = await _service.AgregarItem(dto);
        return Ok(inventario);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> ActualizarInventario([FromRoute] Guid id, [FromBody] ActualizarInventarioDto dto)
    {
        string? userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid usuarioId))
            return Unauthorized(new { message = "Token inválido." });

        await _service.ActualizarInventario(id, dto, usuarioId);
        return NoContent();
    }

    [HttpPost("{id:guid}/usar")]
    [Authorize(Roles = "Player")]
    public async Task<ActionResult> UsarItem(Guid id, [FromQuery] int usos = 1)
    {
        try
        {
            await _service.UsarItem(id, usos);
            return Ok(new { mensaje = "Objeto utilizado y efectos aplicados." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPatch("{id:guid}/equipar")]
    [Authorize(Roles = "Player")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> EquiparItem([FromRoute] Guid id)
    {
        string? userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid usuarioId))
            return Unauthorized(new { message = "Token inválido." });
        
        await _service.EquiparItem(id, usuarioId);
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
    
    [HttpPost("comprar")]
    [Authorize(Roles = "Player")]
    [ProducesResponseType(typeof(InventarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ComprarItem([FromBody] CrearInventarioDto dto)
    {
        try
        {
            string? userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid usuarioId))
                return Unauthorized(new { message = "Token inválido." });
            
            var inventario = await _service.ComprarItem(dto, usuarioId);
            return Ok(inventario);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
    
    [HttpDelete("{id:guid}/vender")]
    [Authorize(Roles = "Player")]
    public async Task<ActionResult> VenderItem(Guid id)
    {
        try
        {
            string? userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid usuarioId))
                return Unauthorized(new { message = "Token inválido." });
            
            await _service.VenderItem(id, usuarioId);
            return Ok(new { mensaje = "Objeto vendido exitosamente." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}
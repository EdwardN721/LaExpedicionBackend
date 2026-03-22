using System.Text.Json;
using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Interfaces;
using LaExpedicion.Application.Parameters;
using LaExpedicion.Shared.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace LaExpedicion.API.Controllers;

/// <summary>
/// Controlador que administra las expediciones
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ExpedicionController : ControllerBase
{
    private readonly IExpedicionService _service;

    public ExpedicionController(IExpedicionService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Obtener todas las expediciones
    /// </summary>
    /// <returns>Lista de expediciones</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedList<ExpedicionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> ObtenerTodas([FromQuery] ExpedicionParameters parameters)
    {
        PagedList<ExpedicionDto> pagedResult = await _service.ObtenerTodasExpediciones(parameters);
        
        string metadata = JsonSerializer.Serialize(pagedResult.Metadata);
        
        Response.Headers.Append("Access-Control-Expose-Headers", "X-Pagination");
        Response.Headers.Append("X-Pagination", metadata);
        
        return Ok(pagedResult);
    }

    /// <summary>
    /// Obtener una expedicion por su Id
    /// </summary>
    /// <param name="id">Id de la expedición</param>
    /// <returns>Expedicion</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ExpedicionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ObtenerPorId(Guid id)
    {
        ExpedicionDto expedicion = await _service.ObtenerExpedicionPorId(id);
        return Ok(expedicion);
    }

    /// <summary>
    /// Crear una expedición
    /// </summary>
    /// <param name="dto">Datos para crear una expedición</param>
    /// <returns>Expedición creada</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ExpedicionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Crear([FromBody] CrearExpedicionDto dto)
    {
        ExpedicionDto expedicion = await _service.CrearExpedicion(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = expedicion.Id }, expedicion);
    }

    /// <summary>
    /// Actualizar una expedicion
    /// </summary>
    /// <param name="id">Id de la expedicion</param>
    /// <param name="dto">Información de la expedicion</param>
    /// <returns>Estado de la actualización</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar([FromRoute] Guid id, [FromBody] ActualizarExpedicionDto dto)
    {
        await _service.ActualizarExpedicion(id, dto);
        return NoContent();
    }

    /// <summary>
    /// Eliminar una expedicion por su Id
    /// </summary>
    /// <param name="id">Id de la expedición</param>
    /// <returns>Estado de la eliminación</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar([FromRoute] Guid id)
    {
        await _service.EliminarExpedicion(id);
        return NoContent();
    }
}
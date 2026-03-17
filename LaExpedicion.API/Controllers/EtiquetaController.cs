using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using LaExpedicion.Application.Interfaces;
using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Parameters;
using LaExpedicion.Shared.Pagination;

namespace LaExpedicion.API.Controllers;

/// <summary>
/// Controlador para administrar las etiquetas
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EtiquetaController : ControllerBase
{
    private readonly IEtiquetaService _service;

    public EtiquetaController(IEtiquetaService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Obtener todas las etiquetas
    /// </summary>
    /// <returns>Lista de etiquetas</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<EtiquetaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> ObtenerEtiquetas([FromQuery] ItemParameters parameters)
    {
        PagedList<EtiquetaDto> pagedResult = await _service.ObtenerEtiquetas(parameters);

        string metadata = JsonSerializer.Serialize(pagedResult.Metadata);

        Response.Headers.Append("Access-Control-Expose-Headers", "X-Pagination");
        
        Response.Headers.Append("X-Pagination", metadata);
        
        return Ok(pagedResult);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(EtiquetaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ObtenerEtiquetaPorId(Guid id)
    {
        EtiquetaDto etiqueta = await _service.ObtenerEtiquetaPorId(id);
        return Ok(etiqueta);
    }

    /// <summary>
    /// Crear etiqueta
    /// </summary>
    /// <param name="dto">Información para crear la etiqueta</param>
    /// <returns>Etiqueta creada</returns>
    [HttpPost]
    [ProducesResponseType(typeof(EtiquetaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AgregarEtiqueta([FromBody] CrearEtiquetaDto dto)
    {
        EtiquetaDto nuevaEtiqueta = await _service.CrearEtiqueta(dto);
        return CreatedAtAction(nameof(ObtenerEtiquetaPorId), new { id = nuevaEtiqueta.Id }, nuevaEtiqueta);
    }

    /// <summary>
    /// Actualizar etiqueta
    /// </summary>
    /// <param name="id">Id de la etiqueta</param>
    /// <param name="dto">Información para actualizar la etiqueta</param>
    /// <returns>Estado de la actualizacion</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ActualizarEtiqueta([FromRoute] Guid id, [FromBody] ActualizarEtiquetaDto dto)
    {
        await _service.ActualizarEtiqueta(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EliminarEtiqueta(Guid id)
    {
        await _service.EliminarEtiqueta(id);
        return NoContent();
    }
    
}
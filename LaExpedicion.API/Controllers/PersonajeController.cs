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
/// Controlador que administra a los personajes
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PersonajeController : ControllerBase
{
    private readonly IPersonajeService _service;

    public PersonajeController(IPersonajeService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Obtener todos los personajes
    /// </summary>
    /// <returns>Listado de personajes</returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(IEnumerable<PersonajeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ObtenerTodosPeronsajes([FromQuery] ItemParameters parameters)
    {
        PagedList<PersonajeDto> pagedResult = await _service.ObtenerTodosPersonajes(parameters);

        string metadata = JsonSerializer.Serialize(pagedResult.Metadata);

        Response.Headers.Append("Access-Control-Expose-Headers", "X-Pagination");

        Response.Headers.Append("X-Pagination", metadata);

        return Ok(pagedResult);
    }

    /// <summary>
    /// Obtener personaje
    /// </summary>
    /// <param name="id">Id del personaje</param>
    /// <returns>Personaje</returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "Player")]
    [ProducesResponseType(typeof(PersonajeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ObtenerPeronsajePorId(Guid id)
    {
        PersonajeDto personaje = await _service.ObtenerPersonajePorId(id);
        return Ok(personaje);
    }

    /// <summary>
    /// Obtener personaje por el Id del Usuario dueño
    /// </summary>
    [HttpGet("usuario/{usuarioId:guid}")]
    [Authorize(Roles = "Player")]
    [ProducesResponseType(typeof(PersonajeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ObtenerPersonajePorUsuario(Guid usuarioId)
    {
        PersonajeDto personaje = await _service.ObtenerPersonajePorUsuarioId(usuarioId);
        return Ok(personaje);
    }


    /// <summary>
    /// Crear personaje
    /// </summary>
    /// <param name="dto">Información para crear personaje</param>
    /// <returns>Personaje creado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PersonajeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CrearPersonaje([FromBody] CrearPersonajeDto dto)
    {
        PersonajeDto personaje = await _service.CrearPersonaje(dto);
        return CreatedAtAction(nameof(ObtenerPeronsajePorId), new { id = personaje.Id }, personaje);
    }

    /// <summary>
    /// Actulizar información del personaje  
    /// </summary>
    /// <param name="id">Id del personaje</param>
    /// <param name="dto">Información del personaje a actualizar</param>
    /// <returns>Estado de la actualización</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActualizarPersonaje([FromRoute] Guid id, [FromBody] ActualizarPersonajeDto dto)
    {
        string? userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid usuarioId))
            return Unauthorized(new { message = "Token inválido." });
        
        await _service.ActualizarPersonaje(id, dto, usuarioId);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EliminarPersonaje([FromRoute] Guid id)
    {
        string? userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid usuarioId))
            return Unauthorized(new { message = "Token inválido." });
        
        await _service.EliminarPersonaje(id, usuarioId);
        return NoContent();
    }
}
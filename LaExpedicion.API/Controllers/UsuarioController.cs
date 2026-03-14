using Microsoft.AspNetCore.Mvc;
using LaExpedicion.Application.Interfaces;
using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;

namespace LaExpedicion.API.Controllers;

/// <summary>
/// Controlador para administrar a los usuarios
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    /// <summary>
    /// Constructor del controlador de usuarios
    /// </summary>
    /// <param name="usuarioService">Lógica del usuario</param>
    /// <exception cref="ArgumentNullException">Excepcion lanzada en caso de no éxistir</exception>
    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
    }

    /// <summary>
    /// Obtener todos los usuarios
    /// </summary>
    /// <returns>Lista de usuarios</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), 200)]
    public async Task<IActionResult> ObtenerUsuarios()
    {
        IEnumerable<UsuarioDto> usuarios = await _usuarioService.ObtenerUsuarios();
        return Ok(usuarios);
    }

    /// <summary>
    /// Obtener un usuario por su Id
    /// </summary>
    /// <param name="id">Id del usuario a buscar</param>
    /// <returns>Usuario</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UsuarioDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ObtenerUsuarioPorId(Guid id)
    {
        UsuarioDto usuario = await _usuarioService.ObtenerUsuarioPorId(id);
        return Ok(usuario);
    }

    /// <summary>
    /// Crear un usuario
    /// </summary>
    /// <param name="dto">Objeto de creación del usuario</param>
    /// <returns>Usuario creado.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CrearUsuario([FromBody] CrearUsuarioDto dto)
    {
        UsuarioDto usuarioCreado = await _usuarioService.AgregarUsuario(dto);
        return CreatedAtAction(nameof(ObtenerUsuarioPorId), new { id = usuarioCreado.Id }, usuarioCreado);
    }

    /// <summary>
    /// Actualizar a un usuario
    /// </summary>
    /// <param name="id">Id del usuario a buscar</param>
    /// <param name="dto">Informacion del usuario para actualizar</param>
    /// <returns>Respuesta de la operacion</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> ActualizarUsuario([FromRoute] Guid id, [FromBody] ActualizarUsuarioDto dto)
    {
        await _usuarioService.ActualizarUsuario(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> EliminarUsuario([FromRoute] Guid id)
    {
        await _usuarioService.EliminarUsuario(id);
        return NoContent();
    }
}
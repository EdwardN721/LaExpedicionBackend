using System.Text.Json;
using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Interfaces;
using LaExpedicion.Application.Parameters;
using LaExpedicion.Shared.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace LaExpedicion.API.Controllers;

/// <summary>
/// Controlador que administra los items
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ItemController : ControllerBase
{
    private readonly IItemService _service;

    public ItemController(IItemService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Obtener todos los Items
    /// </summary>
    /// <param name="itemParameters">Parametros para paginar</param>
    /// <returns>Lista de items</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<ItemDto>), 200)]
    public async Task<IActionResult> ObtenerTodosItems([FromQuery] ItemParameters itemParameters)
    {
        // Llamamos al servicio pasando los parámetros
        PagedList<ItemDto> pagedResult = await _service.ObtenerTodosItems(itemParameters);

        // Serializamos la Metadata a JSON
        var metadataJson = JsonSerializer.Serialize(pagedResult.Metadata);
        
        // Agregamos un cabecero que le indica al Frontend/Swagger 
        // que es seguro leer el cabecero X-Pagination (por temas de CORS)
        Response.Headers.Append("Access-Control-Expose-Headers", "X-Pagination");
        
        // Agregamos nuestro cabecero con la información de paginación
        Response.Headers.Append("X-Pagination", metadataJson);

        // Devolvemos el array limpio en el body
        return Ok(pagedResult);
    }

    /// <summary>
    /// Obtener un Item
    /// </summary>
    /// <param name="id">Id del Item</param>
    /// <returns>Item</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ItemDto), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerItem([FromRoute] Guid id)
    {
        ItemDto item = await _service.ObtenerItemPorId(id);
        return Ok(item);
    }

    /// <summary>
    /// Crear un Item
    /// </summary>
    /// <param name="itemDto">Información para crear un Item</param>
    /// <returns>Item Creado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ItemDto), 201)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CrearItem([FromBody] CrearItemDto itemDto)
    {
        ItemDto item = await _service.CrearItem(itemDto);
        return CreatedAtAction(nameof(ObtenerItem), new { id = item.Id }, item);
    }
    
    /// <summary>
    /// Actualizar un Item
    /// </summary>
    /// <param name="id">Id del iten</param>
    /// <param name="itemDto">Información del Item</param>
    /// <returns>Estado de la actualización</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ActualizarItem([FromRoute] Guid id, [FromBody] ActualizarItemDto itemDto)
    {
        await _service.ModificarItem(id, itemDto);
        return NoContent();
    }

    /// <summary>
    /// Eliminar un Item
    /// </summary>
    /// <param name="id">Id del Item</param>
    /// <returns>Estado de la elimiación</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> EliminarItem([FromRoute] Guid id)
    {
        await _service.EliminarItem(id);
        return NoContent();
    }
    
}
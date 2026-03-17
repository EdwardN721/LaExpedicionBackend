using System.Text.Json;
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
    
    
}
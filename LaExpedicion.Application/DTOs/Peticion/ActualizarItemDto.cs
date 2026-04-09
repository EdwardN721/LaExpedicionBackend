using Microsoft.AspNetCore.Http;

namespace LaExpedicion.Application.DTOs.Peticion;

public class ActualizarItemDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public double Precio { get; set; }
    public int TipoItem { get; set; }
    public bool Activo { get; set; }
    public IFormFile? Imagen { get; set; }
}
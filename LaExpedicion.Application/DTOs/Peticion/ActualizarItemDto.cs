namespace LaExpedicion.Application.DTOs.Peticion;

public class ActualizarItemDto
{
    public Guid Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Descripcion { get; init; } = string.Empty;
    public double Precio { get; set; }
}
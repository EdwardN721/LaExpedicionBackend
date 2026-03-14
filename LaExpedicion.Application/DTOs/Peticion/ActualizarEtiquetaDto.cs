namespace LaExpedicion.Application.DTOs.Peticion;

public class ActualizarEtiquetaDto
{
    public Guid Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Descripcion { get; init; } = string.Empty;
}
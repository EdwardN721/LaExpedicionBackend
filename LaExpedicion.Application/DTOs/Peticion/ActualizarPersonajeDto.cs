namespace LaExpedicion.Application.DTOs.Peticion;

public record ActualizarPersonajeDto
{
    public Guid Id { get; init; }
    public string NombreUsuario { get; init; } = string.Empty;
    public Guid EtiquetaId { get; init; }
};
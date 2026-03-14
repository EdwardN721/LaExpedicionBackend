namespace LaExpedicion.Application.DTOs.Peticion;

public record CrearPersonajeDto
{
    public Guid UsuarioId { get; init; }
    public string NombreUsuario { get; init; } = string.Empty;
};
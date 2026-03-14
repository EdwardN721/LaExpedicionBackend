namespace LaExpedicion.Application.DTOs.Peticion;

public record ActualizarUsuarioDto
{
    public string Nombre { get; init; } = string.Empty;
    public string PrimerApellido { get; init; } = string.Empty;
    public string? SegundoApellido { get; init; } = string.Empty;
    public string Telefono { get; init; } = string.Empty;
}
namespace LaExpedicion.Application.DTOs.Peticion;

public record CrearUsuarioDto
{
    private string Nombre { get; init; } = string.Empty;
    private string PrimerApellido { get; init; } = string.Empty;
    private string? SegundoApellido { get; init; } = string.Empty;
    private string Correo { get; init; } = string.Empty;
    private string Password { get; init; } = string.Empty;
    private string Telefono { get; init; } = string.Empty;
};
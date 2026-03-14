namespace LaExpedicion.Application.DTOs.Respuesta;

public record UsuarioDto
{
    public Guid Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string PrimerApellido { get; init; } = string.Empty;
    public string SegundoApellido { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
};
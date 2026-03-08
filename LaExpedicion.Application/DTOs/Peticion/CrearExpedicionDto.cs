namespace LaExpedicion.Application.DTOs.Peticion;

public record CrearExpedicionDto
{
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; } = string.Empty;
    public int Experiencia { get; init; } 
    public double Dinero { get; init; }
};
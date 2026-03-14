namespace LaExpedicion.Application.DTOs.Peticion;

public class ActualizarInventarioDto
{
    public Guid Id { get; init; }
    // Solo permitimos actualizar si está equipado o cuántos usos le quedan
    public bool Equipado { get; init; }
    public int UsosRestantes { get; init; }   
}
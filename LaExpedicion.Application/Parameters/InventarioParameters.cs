namespace LaExpedicion.Application.Parameters;

public class InventarioParameters : RequestParameters
{
    public string? Nombre { get; set; }
    public bool Equipado { get; set; }
    public int UsosRestantes { get; set; }
}
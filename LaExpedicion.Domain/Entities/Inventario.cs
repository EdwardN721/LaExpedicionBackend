namespace LaExpedicion.Domain.Entities;

public class Inventario : BaseEntity
{
    public Guid PersonajeId { get; set; }
    public Guid ItemId { get; set; }
    public bool Equipado { get; set; }
    public int UsosRestantes { get; set; }
    
    public Personaje? Personaje { get; set; }
    public Item? Item { get; set; }
}
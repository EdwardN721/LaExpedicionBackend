namespace LaExpedicion.Domain.Entities;

public class Estadistica : BaseEntity
{
    public Guid PersonajeId { get; set; }
    public int Salud { get; set; }
    public int Fuerza { get; set; }
    public int Energia { get; set; }
    public int Magia { get; set; }
    public int Mana { get; set; }
    
    public Personaje? Personaje { get; set; }
}
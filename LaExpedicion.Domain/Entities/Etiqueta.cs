namespace LaExpedicion.Domain.Entities;

public class Etiqueta : BaseEntity
{
    public string Nombre { get; set; } = String.Empty;
    public string Descripcion { get; set; } = String.Empty;
    
    public virtual ICollection<Personaje> Personajes { get; set; } = new List<Personaje>();
}
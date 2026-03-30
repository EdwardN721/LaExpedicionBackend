using LaExpedicion.Domain.Enum;

namespace LaExpedicion.Domain.Entities;

public class Item : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; } = string.Empty;
   
    public double Precio { get; set; }
    
    public virtual ICollection<ItemModificador> ItemModificador { get; set; } = new HashSet<ItemModificador>();
    public virtual ICollection<Inventario> Inventario { get; set; } = new HashSet<Inventario>();
}
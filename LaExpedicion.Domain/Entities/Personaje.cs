namespace LaExpedicion.Domain.Entities;

public class Personaje : BaseEntity
{
    public Guid UsuarioId { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public Guid EtiquetaId { get; set; }
    
    public Usuario? Usuario { get; set; }
    public Etiqueta? Etiqueta { get; set; }
    public Estadistica? Estadistica { get; set; }
    
    public virtual ICollection<Inventario> Inventario { get; set; } = new HashSet<Inventario>(); 
}
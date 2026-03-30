namespace LaExpedicion.Domain.Entities;

public class Personaje : BaseEntity
{
    public Guid UsuarioId { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    
    public int Nivel { get; set; } = 1;
    public int Experiencia { get; set; } = 0;
    public double Dinero { get; set; } = 0;
    public int SaludActual { get; set; }
    
    public Guid EtiquetaId { get; set; }
    public Etiqueta? Etiqueta { set; get; }
    public Usuario? Usuario { get; set; }
    public Estadistica? Estadistica { get; set; }
    
    public virtual ICollection<Inventario> Inventario { get; set; } = new HashSet<Inventario>(); 
}
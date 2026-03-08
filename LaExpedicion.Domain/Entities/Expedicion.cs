namespace LaExpedicion.Domain.Entities;

public class Expedicion : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int Experiencia { get; set; }
    public double Dinero { get; set; }
    
    public virtual ICollection<ExpedicionRealizada> ExpedicionesRealizadas { get; set; } = new HashSet<ExpedicionRealizada>();
}
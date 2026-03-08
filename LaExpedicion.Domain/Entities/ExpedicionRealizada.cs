using LaExpedicion.Domain.Enum;

namespace LaExpedicion.Domain.Entities;

public class ExpedicionRealizada : BaseEntity
{
    public Guid PersonajeId { get; set; }
    public Guid ExpedicionId { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public EnumResultado Resultado { get; set; }
    public int ExperienciaGanada { get; set; }
    public double DineroGanado { get; set; }
    
    public Personaje? Personaje { get; set; }
    public Expedicion? Expedicion { get; set; }
}
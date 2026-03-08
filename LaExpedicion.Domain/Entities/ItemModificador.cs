using LaExpedicion.Domain.Enum;

namespace LaExpedicion.Domain.Entities;

public class ItemModificador : BaseEntity
{
    public Guid ItemId { get; set; }
    public EnumEstadistica EstadisticaAfectada { get; set; }
    public int ValorAjuste { get; set; }
    
    public Item? Item { get; set; }
}
namespace LaExpedicion.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    public bool Activo { get; set; }
    public bool Eliminado { get; set; }
    public string UsuarioCreacion { get; set; } = string.Empty;
    public string UsuarioModificacion { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaModificacion { get; set; }
}
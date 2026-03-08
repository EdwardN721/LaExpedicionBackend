using Microsoft.AspNetCore.Identity;

namespace LaExpedicion.Domain.Entities;

public class Usuario : IdentityUser<Guid>
{
    public string Nombre { get ; set; } = string.Empty;
    public string PrimerApellido { get ; set; } = string.Empty;  
    public string SegundoApellido { get ; set; } = string.Empty;  
}
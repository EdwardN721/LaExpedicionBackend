using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;

namespace LaExpedicion.Application.Interfaces;

public interface IUsuarioService
{
    public Task<IEnumerable<UsuarioDto>> ObtenerUsuarios();
    public Task<UsuarioDto> ObtenerUsuarioPorId(Guid id);
    public Task<UsuarioDto> AgregarUsuario(CrearUsuarioDto dto);
    public Task ActualizarUsuario(Guid id, ActualizarUsuarioDto dto);
    public Task EliminarUsuario(Guid id);
}
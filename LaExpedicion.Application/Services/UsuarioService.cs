using LaExpedicion.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LaExpedicion.Application.Mappers;
using LaExpedicion.Application.Interfaces;
using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace LaExpedicion.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly UserManager<Usuario> _userManager;
    private readonly ILogger<UsuarioService> _logger;
    
    public UsuarioService(UserManager<Usuario> userManager, ILogger<UsuarioService> logger)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<IEnumerable<UsuarioDto>> ObtenerUsuarios()
    {
        List<Usuario> usuarios = await _userManager.Users.ToListAsync();
        _logger.LogInformation("Usuarios obtenidos: {Usuarios}", usuarios.Count);
        return usuarios.MapToDto();
    }

    public async Task<UsuarioDto> ObtenerUsuarioPorId(Guid id)
    {
        Usuario usuario = await UsuarioPorId(id);

        return usuario.MapToDto();
    }

    public async Task<UsuarioDto> AgregarUsuario(CrearUsuarioDto dto)
    {
        // Crear la instancia
        Usuario usuario = dto.MapToEntity();
        
        // Crear un usuario con su contraseña hasheada automaticanente 
        IdentityResult crearUsuario = await _userManager.CreateAsync(usuario, dto.Password);
        
        // Validar creación
        if (!crearUsuario.Succeeded)
        {
            string errores = ValidarErrores(crearUsuario);
            _logger.LogError("Error crear usuario: {Error}", errores);
            throw new Exception($"El usuario no se agrego correctamente: {errores}");
        }
        
        await _userManager.AddToRoleAsync(usuario, "Player");
        
        _logger.LogInformation("Usuario agregado correctamente: {UsuarioNombre}", usuario.Nombre);
        return usuario.MapToDto();
    }

    public async Task ActualizarUsuario(Guid id, ActualizarUsuarioDto dto)
    {
        Usuario usuario = await UsuarioPorId(id);
        
        usuario.UpdateEntity(dto);
        
        IdentityResult actualizarUsuario = await _userManager.UpdateAsync(usuario);

        if (!actualizarUsuario.Succeeded)
        {
            string errores = ValidarErrores(actualizarUsuario);
            _logger.LogError("Error actualizar usuario: {Error}", errores);
            throw new Exception($"El usuario no se actualizó: {errores}");
        }
        _logger.LogInformation("Usuario actualizado correctamente: {UsuarioNombre}", usuario.Nombre);
    }

    public async Task EliminarUsuario(Guid id)
    {
        Usuario usuario = await UsuarioPorId(id);
        
        IdentityResult eliminarUsuario = await _userManager.DeleteAsync(usuario);

        if (!eliminarUsuario.Succeeded)
        {
            string errores = ValidarErrores(eliminarUsuario);
            _logger.LogError("Error eliminar usuario: {Error}", errores);
            throw new Exception($"Error al eliminar usuario: {errores}");
        }
        _logger.LogWarning("Usuario eliminado correctamente: {UsuarioNombre}", usuario.Nombre);
    }

    public async Task<Usuario?> Login(LoginDto dto)
    {
        Usuario? usuario = await _userManager.FindByEmailAsync(dto.Email);

        if (usuario == null)
        {
            _logger.LogWarning("Usuario no encontrado: {Email}", dto.Email);
            return null;
        }
        
        bool resultado = await _userManager.CheckPasswordAsync(usuario, dto.Password);
        if (!resultado)
        {
            _logger.LogWarning("Credenciales incorrectas.");
            return null;
        }

        return usuario;
    }

    #region MetodosPrivados

    private async Task<Usuario> UsuarioPorId(Guid id)
    {
        Usuario? usuario = await _userManager.FindByIdAsync(id.ToString());
        if (usuario == null)
        {
            _logger.LogWarning("Usuario no encontrado: {Id}", id);
            throw new NotFoundException($"Usuario no encontrado con el Id: {id}");
        }
        
        return usuario;
    }

    private string ValidarErrores(IdentityResult result)
    {
        var errores = result.Errors.Select(e => $"{e.Code}: {e.Description}");
        
        return string.Join(" | ", errores);
    }

    #endregion
}
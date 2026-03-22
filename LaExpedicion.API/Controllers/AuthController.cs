using Microsoft.AspNetCore.Mvc;
using LaExpedicion.Domain.Entities;
using LaExpedicion.Application.Interfaces;
using LaExpedicion.Application.DTOs.Peticion;

namespace LaExpedicion.API.Controllers;

/// <summary>
/// Controlador que administra el login
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly ITokenService _tokenService;

    public AuthController(IUsuarioService usuarioService, ITokenService tokenService)
    {
        _tokenService = tokenService;
        _usuarioService = usuarioService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        Usuario? usuario = await _usuarioService.Login(loginDto);
        if (usuario == null) return Unauthorized("Credenciales incorrectas.");
        
        string token = await _tokenService.CreateToken(usuario);
        return Ok(new
        {
            usuario.Id,
            usuario.Email,
            usuario.UserName,
            Token = token
        });
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LaExpedicion.Application.Interfaces;
using LaExpedicion.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace LaExpedicion.Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<Usuario> _userManager;
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration configuration, UserManager<Usuario> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
    }

    public async Task<string> CreateToken(Usuario usuario)
    {
        // 1. Datos que irán dentro del Token (Claims)
        List<Claim> claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email!)
        };
        
        // 2. Agregamos los roles del usuario al Token
        IList<string> roles = await _userManager.GetRolesAsync(usuario);
        foreach (var rol in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, rol));
        }
        
        // 3. Credenciales para firmar token
        SigningCredentials creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
        
        // 4. Descripcion del token
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"]!)),
            SigningCredentials = creds,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
        };
        
        // 5. Fabricamos el token
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
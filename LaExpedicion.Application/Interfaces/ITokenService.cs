using LaExpedicion.Domain.Entities;

namespace LaExpedicion.Application.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(Usuario usuario);
}
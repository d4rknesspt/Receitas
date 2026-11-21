using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Receitas.DAL.Repositories;
using Receitas.Models;
using Receitas.Models.Dtos;

namespace Receitas.Services;

public class AuthService : IAuthService
{
    private readonly IUtilizadorRepository _repositorio;
    private readonly JwtSettings _jwt;
    private readonly PasswordHasher<Utilizador> _hasher = new();

    public AuthService(IUtilizadorRepository repositorio, IOptions<JwtSettings> jwt)
    {
        _repositorio = repositorio;
        _jwt = jwt.Value;
    }

    public async Task<AuthRespostaDto?> RegistarAsync(RegistoDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        if (await _repositorio.EmailExisteAsync(email))
        {
            return null;
        }

        var utilizador = new Utilizador
        {
            Nome = dto.Nome.Trim(),
            Email = email,
            Perfil = Perfil.Utilizador
        };
        utilizador.PasswordHash = _hasher.HashPassword(utilizador, dto.Password);

        await _repositorio.CriarAsync(utilizador);

        return GerarResposta(utilizador);
    }

    public async Task<AuthRespostaDto?> LoginAsync(LoginDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        var utilizador = await _repositorio.ObterPorEmailAsync(email);

        if (utilizador == null || utilizador.Bloqueado)
        {
            return null;
        }

        var resultado = _hasher.VerifyHashedPassword(utilizador, utilizador.PasswordHash, dto.Password);
        if (resultado == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return GerarResposta(utilizador);
    }

    private AuthRespostaDto GerarResposta(Utilizador utilizador)
    {
        var expiraEm = DateTime.UtcNow.AddMinutes(_jwt.ExpiraEmMinutos);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, utilizador.Id.ToString()),
            new(ClaimTypes.Name, utilizador.Nome),
            new(ClaimTypes.Email, utilizador.Email),
            new(ClaimTypes.Role, utilizador.Perfil.ToString())
        };

        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: expiraEm,
            signingCredentials: credenciais);

        return new AuthRespostaDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiraEm = expiraEm,
            Nome = utilizador.Nome,
            Email = utilizador.Email,
            Perfil = utilizador.Perfil.ToString()
        };
    }
}
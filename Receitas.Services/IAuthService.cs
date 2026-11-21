using Receitas.Models.Dtos;

namespace Receitas.Services;

public interface IAuthService
{
    Task<AuthRespostaDto?> RegistarAsync(RegistoDto dto);
    Task<AuthRespostaDto?> LoginAsync(LoginDto dto);
}
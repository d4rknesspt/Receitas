using Receitas.Models;

namespace Receitas.DAL.Repositories;

public interface IUtilizadorRepository
{
    Task<Utilizador?> ObterPorEmailAsync(string email);
    Task<bool> EmailExisteAsync(string email);
    Task<Utilizador> CriarAsync(Utilizador utilizador);
}
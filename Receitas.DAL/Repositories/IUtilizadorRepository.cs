using Receitas.Models;

namespace Receitas.DAL.Repositories;

public interface IUtilizadorRepository
{
    Task<Utilizador?> ObterPorEmailAsync(string email);
    Task<bool> EmailExisteAsync(string email);
    Task<Utilizador> CriarAsync(Utilizador utilizador);
    Task<List<Utilizador>> ObterTodosAsync();
    Task<Utilizador?> ObterPorIdAsync(int id);
    Task<bool> DefinirBloqueioAsync(int id, bool bloqueado);
}
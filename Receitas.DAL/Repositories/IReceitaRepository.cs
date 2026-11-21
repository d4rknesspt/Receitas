using Receitas.Models;

namespace Receitas.DAL.Repositories;

public interface IReceitaRepository
{
    Task<List<Receita>> ObterTodasAsync();
    Task<Receita?> ObterPorIdAsync(int id);
    Task<Receita> CriarAsync(Receita receita);
    Task<bool> AtualizarAsync(Receita receita);
    Task<bool> ApagarAsync(int id);
    Task<bool> DefinirValidacaoAsync(int id, bool validada);
}
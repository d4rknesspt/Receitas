using Receitas.Models;

namespace Receitas.DAL.Repositories;

public interface ICatalogoRepository
{
    Task<List<Categoria>> ObterCategoriasAsync();
    Task<List<Ingrediente>> ObterIngredientesValidadosAsync();
    Task<List<Ingrediente>> ObterIngredientesPendentesAsync();
    Task<List<Ingrediente>> ObterIngredientesPorIdsAsync(List<int> ids);
    Task<Ingrediente?> ObterIngredientePorIdAsync(int id);
    Task<bool> IngredienteExisteAsync(string nome, int? ignorarId = null);
    Task<Ingrediente> CriarIngredienteAsync(Ingrediente ingrediente);
    Task AtualizarIngredienteAsync(Ingrediente ingrediente);
    Task ApagarIngredienteAsync(Ingrediente ingrediente);
    Task<bool> ValidarIngredienteAsync(int id);
}
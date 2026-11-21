using Receitas.Models;

namespace Receitas.DAL.Repositories;

public interface ICatalogoRepository
{
    Task<List<Categoria>> ObterCategoriasAsync();
    Task<List<Ingrediente>> ObterIngredientesValidadosAsync();
    Task<List<Ingrediente>> ObterIngredientesPendentesAsync();
    Task<bool> IngredienteExisteAsync(string nome);
    Task<Ingrediente> CriarIngredienteAsync(Ingrediente ingrediente);
    Task<bool> ValidarIngredienteAsync(int id);
}
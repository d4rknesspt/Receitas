using Receitas.Models;

namespace Receitas.DAL.Repositories;

public interface ICatalogoRepository
{
    Task<List<Categoria>> ObterCategoriasAsync();
    Task<List<Ingrediente>> ObterIngredientesValidadosAsync();
}
using Receitas.Models.Dtos;

namespace Receitas.Services;

public interface ICatalogoService
{
    Task<List<CategoriaDto>> ObterCategoriasAsync();
    Task<List<IngredienteDto>> ObterIngredientesAsync();
}
using Receitas.Models.Dtos;

namespace Receitas.Services;

public interface ICatalogoService
{
    Task<List<CategoriaDto>> ObterCategoriasAsync();
    Task<List<IngredienteDto>> ObterIngredientesAsync();
    Task<List<IngredienteGestaoDto>> ObterIngredientesPendentesAsync();
    Task<IngredienteGestaoDto?> CriarIngredienteAsync(IngredienteCriarDto dto, bool eAdmin);
    Task<bool> ValidarIngredienteAsync(int id);
}
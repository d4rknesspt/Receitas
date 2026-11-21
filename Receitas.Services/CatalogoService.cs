using Receitas.DAL.Repositories;
using Receitas.Models.Dtos;

namespace Receitas.Services;

public class CatalogoService : ICatalogoService
{
    private readonly ICatalogoRepository _repositorio;

    public CatalogoService(ICatalogoRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<List<CategoriaDto>> ObterCategoriasAsync()
    {
        var categorias = await _repositorio.ObterCategoriasAsync();

        return categorias
            .Select(c => new CategoriaDto { Id = c.Id, Nome = c.Nome })
            .ToList();
    }

    public async Task<List<IngredienteDto>> ObterIngredientesAsync()
    {
        var ingredientes = await _repositorio.ObterIngredientesValidadosAsync();

        return ingredientes
            .Select(i => new IngredienteDto { Id = i.Id, Nome = i.Nome })
            .ToList();
    }
}
using Receitas.DAL.Repositories;
using Receitas.Models;
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

    public async Task<List<IngredienteGestaoDto>> ObterIngredientesPendentesAsync()
    {
        var ingredientes = await _repositorio.ObterIngredientesPendentesAsync();

        return ingredientes.Select(ParaGestaoDto).ToList();
    }

    public async Task<IngredienteGestaoDto?> CriarIngredienteAsync(IngredienteCriarDto dto, bool eAdmin)
    {
        var nome = dto.Nome.Trim();

        if (await _repositorio.IngredienteExisteAsync(nome))
        {
            return null;
        }

        var criado = await _repositorio.CriarIngredienteAsync(new Ingrediente
        {
            Nome = nome,
            Validado = eAdmin
        });

        return ParaGestaoDto(criado);
    }

    public Task<bool> ValidarIngredienteAsync(int id)
    {
        return _repositorio.ValidarIngredienteAsync(id);
    }

    private static IngredienteGestaoDto ParaGestaoDto(Ingrediente i)
    {
        return new IngredienteGestaoDto
        {
            Id = i.Id,
            Nome = i.Nome,
            Validado = i.Validado
        };
    }
}
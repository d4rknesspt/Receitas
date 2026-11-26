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
            .Select(i => new IngredienteDto
            {
                Id = i.Id,
                Nome = i.Nome,
                TipoMedida = i.TipoMedida,
                Unidades = Medidas.UnidadesPermitidas[i.TipoMedida]
            })
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
            TipoMedida = dto.TipoMedida,
            Validado = eAdmin
        });

        return ParaGestaoDto(criado);
    }

    public async Task<IngredienteGestaoDto?> AtualizarIngredienteAsync(int id, IngredienteCriarDto dto)
    {
        var ingrediente = await _repositorio.ObterIngredientePorIdAsync(id);

        if (ingrediente == null)
        {
            return null;
        }

        if (ingrediente.Validado)
        {
            throw new RegraNegocioException("Só é possível alterar ingredientes que ainda não foram validados.");
        }

        var nome = dto.Nome.Trim();

        if (await _repositorio.IngredienteExisteAsync(nome, id))
        {
            throw new RegraNegocioException("Já existe outro ingrediente com esse nome.");
        }

        ingrediente.Nome = nome;
        ingrediente.TipoMedida = dto.TipoMedida;

        await _repositorio.AtualizarIngredienteAsync(ingrediente);
        return ParaGestaoDto(ingrediente);
    }

    public async Task<bool> RecusarIngredienteAsync(int id)
    {
        var ingrediente = await _repositorio.ObterIngredientePorIdAsync(id);

        if (ingrediente == null)
        {
            return false;
        }

        if (ingrediente.Validado)
        {
            throw new RegraNegocioException("Só é possível recusar ingredientes que ainda não foram validados.");
        }

        await _repositorio.ApagarIngredienteAsync(ingrediente);
        return true;
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
            Validado = i.Validado,
            TipoMedida = i.TipoMedida
        };
    }
}
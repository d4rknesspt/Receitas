using Receitas.DAL.Repositories;
using Receitas.Models;
using Receitas.Models.Dtos;

namespace Receitas.Services;

public class ReceitaService : IReceitaService
{
    private readonly IReceitaRepository _repositorio;

    public ReceitaService(IReceitaRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<List<ReceitaDto>> ObterTodasAsync()
    {
        var receitas = await _repositorio.ObterTodasAsync();
        return receitas.Select(ParaDto).ToList();
    }

    public async Task<ReceitaDto?> ObterPorIdAsync(int id)
    {
        var receita = await _repositorio.ObterPorIdAsync(id);
        return receita == null ? null : ParaDto(receita);
    }

    public async Task<ReceitaDto> CriarAsync(ReceitaGuardarDto dto)
    {
        var receita = ParaEntidade(dto);
        receita.Validada = false;

        var criada = await _repositorio.CriarAsync(receita);

        var completa = await _repositorio.ObterPorIdAsync(criada.Id);
        return ParaDto(completa!);
    }

    public async Task<bool> AtualizarAsync(int id, ReceitaGuardarDto dto)
    {
        var existente = await _repositorio.ObterPorIdAsync(id);
        if (existente == null)
        {
            return false;
        }

        var receita = ParaEntidade(dto);
        receita.Id = id;
        receita.Validada = existente.Validada;

        return await _repositorio.AtualizarAsync(receita);
    }

    public Task<bool> ApagarAsync(int id)
    {
        return _repositorio.ApagarAsync(id);
    }

    private static ReceitaDto ParaDto(Receita r)
    {
        return new ReceitaDto
        {
            Id = r.Id,
            Titulo = r.Titulo,
            ModoPreparacao = r.ModoPreparacao,
            CategoriaId = r.CategoriaId,
            Categoria = r.Categoria?.Nome ?? string.Empty,
            Dificuldade = r.Dificuldade,
            DuracaoMinutos = r.DuracaoMinutos,
            Validada = r.Validada,
            Ingredientes = r.Ingredientes.Select(ri => new ReceitaIngredienteDto
            {
                IngredienteId = ri.IngredienteId,
                Ingrediente = ri.Ingrediente?.Nome ?? string.Empty,
                Quantidade = ri.Quantidade,
                Unidade = ri.Unidade
            }).ToList()
        };
    }

    private static Receita ParaEntidade(ReceitaGuardarDto dto)
    {
        return new Receita
        {
            Titulo = dto.Titulo,
            ModoPreparacao = dto.ModoPreparacao,
            CategoriaId = dto.CategoriaId,
            Dificuldade = dto.Dificuldade,
            DuracaoMinutos = dto.DuracaoMinutos,
            Ingredientes = dto.Ingredientes.Select(i => new ReceitaIngrediente
            {
                IngredienteId = i.IngredienteId,
                Quantidade = i.Quantidade,
                Unidade = i.Unidade
            }).ToList()
        };
    }
}
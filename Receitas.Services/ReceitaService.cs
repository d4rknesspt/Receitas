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

    private static bool PodeVer(Receita receita, int? utilizadorId, bool eAdmin)
    {
        return receita.Validada
            || eAdmin
            || (utilizadorId.HasValue && receita.AutorId == utilizadorId.Value);
    }

    public async Task<List<ReceitaDto>> ObterTodasAsync(int? utilizadorId, bool eAdmin)
    {
        var receitas = await _repositorio.ObterTodasAsync();

        return receitas
            .Where(r => PodeVer(r, utilizadorId, eAdmin))
            .Select(ParaDto)
            .ToList();
    }

    public async Task<ReceitaDto?> ObterPorIdAsync(int id, int? utilizadorId, bool eAdmin)
    {
        var receita = await _repositorio.ObterPorIdAsync(id);

        if (receita == null || !PodeVer(receita, utilizadorId, eAdmin))
        {
            return null;
        }

        return ParaDto(receita);
    }

    public async Task<ReceitaDto> CriarAsync(ReceitaGuardarDto dto, int autorId)
    {
        var receita = ParaEntidade(dto);
        receita.Validada = false;
        receita.AutorId = autorId;

        var criada = await _repositorio.CriarAsync(receita);

        var completa = await _repositorio.ObterPorIdAsync(criada.Id);
        return ParaDto(completa!);
    }

    public async Task<ResultadoOperacao> AtualizarAsync(int id, ReceitaGuardarDto dto, int utilizadorId, bool eAdmin)
    {
        var existente = await _repositorio.ObterPorIdAsync(id);
        if (existente == null)
        {
            return ResultadoOperacao.NaoEncontrada;
        }

        if (!eAdmin && existente.AutorId != utilizadorId)
        {
            return ResultadoOperacao.SemPermissao;
        }

        var receita = ParaEntidade(dto);
        receita.Id = id;
        receita.Validada = existente.Validada;

        var atualizada = await _repositorio.AtualizarAsync(receita);
        return atualizada ? ResultadoOperacao.Sucesso : ResultadoOperacao.NaoEncontrada;
    }

    public async Task<ResultadoOperacao> ApagarAsync(int id, int utilizadorId, bool eAdmin)
    {
        var existente = await _repositorio.ObterPorIdAsync(id);
        if (existente == null)
        {
            return ResultadoOperacao.NaoEncontrada;
        }

        if (!eAdmin && existente.AutorId != utilizadorId)
        {
            return ResultadoOperacao.SemPermissao;
        }

        var apagada = await _repositorio.ApagarAsync(id);
        return apagada ? ResultadoOperacao.Sucesso : ResultadoOperacao.NaoEncontrada;
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
    public Task<bool> ValidarAsync(int id)
    {
        return _repositorio.DefinirValidacaoAsync(id, true);
    }
}
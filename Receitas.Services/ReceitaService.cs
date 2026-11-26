using Receitas.DAL.Repositories;
using Receitas.Models;
using Receitas.Models.Dtos;

namespace Receitas.Services;

public class ReceitaService : IReceitaService
{
    private readonly IReceitaRepository _repositorio;
    private readonly ICatalogoRepository _catalogo;

    public ReceitaService(IReceitaRepository repositorio, ICatalogoRepository catalogo)
    {
        _repositorio = repositorio;
        _catalogo = catalogo;
    }

    private static bool PodeVer(Receita receita, int? utilizadorId, bool eAdmin)
    {
        return receita.Validada
            || eAdmin
            || (utilizadorId.HasValue && receita.AutorId == utilizadorId.Value);
    }

    public async Task<List<ReceitaDto>> ObterTodasAsync(FiltroReceitasDto filtro, int? utilizadorId, bool eAdmin)
    {
        var todas = await _repositorio.ObterTodasAsync();

        var receitas = todas.Where(r => PodeVer(r, utilizadorId, eAdmin));

        if (!string.IsNullOrWhiteSpace(filtro.Texto))
        {
            var texto = filtro.Texto.Trim();
            receitas = receitas.Where(r =>
                r.Titulo.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                r.ModoPreparacao.Contains(texto, StringComparison.OrdinalIgnoreCase));
        }

        if (filtro.CategoriaId.HasValue)
        {
            receitas = receitas.Where(r => r.CategoriaId == filtro.CategoriaId.Value);
        }

        if (filtro.Dificuldade.HasValue)
        {
            receitas = receitas.Where(r => r.Dificuldade == filtro.Dificuldade.Value);
        }

        if (filtro.DuracaoMax.HasValue)
        {
            receitas = receitas.Where(r => r.DuracaoMinutos <= filtro.DuracaoMax.Value);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Ingrediente))
        {
            var ingrediente = filtro.Ingrediente.Trim();
            receitas = receitas.Where(r => r.Ingredientes.Any(ri =>
                ri.Ingrediente.Nome.Contains(ingrediente, StringComparison.OrdinalIgnoreCase)));
        }

        return receitas.Select(ParaDto).ToList();
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
        await ValidarIngredientesAsync(dto);

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

        await ValidarIngredientesAsync(dto);

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

    public Task<bool> ValidarAsync(int id)
    {
        return _repositorio.DefinirValidacaoAsync(id, true);
    }

    private async Task ValidarIngredientesAsync(ReceitaGuardarDto dto)
    {
        var ids = dto.Ingredientes.Select(i => i.IngredienteId).ToList();

        if (ids.Count != ids.Distinct().Count())
        {
            throw new RegraNegocioException("O mesmo ingrediente não pode aparecer duas vezes na receita.");
        }

        var ingredientes = await _catalogo.ObterIngredientesPorIdsAsync(ids);

        foreach (var linha in dto.Ingredientes)
        {
            var ingrediente = ingredientes.FirstOrDefault(i => i.Id == linha.IngredienteId);

            if (ingrediente == null || !ingrediente.Validado)
            {
                throw new RegraNegocioException("Foi escolhido um ingrediente inexistente ou ainda não validado.");
            }

            if (!Medidas.UnidadeValida(ingrediente.TipoMedida, linha.Unidade))
            {
                var permitidas = string.Join(", ", Medidas.UnidadesPermitidas[ingrediente.TipoMedida]);
                throw new RegraNegocioException(
                    $"A unidade \"{linha.Unidade}\" não é válida para {ingrediente.Nome}. Use: {permitidas}.");
            }

            if (!Medidas.QuantidadeValida(ingrediente.TipoMedida, linha.Quantidade))
            {
                var regra = ingrediente.TipoMedida == TipoMedida.Unidade
                    ? "tem de ser um número inteiro"
                    : "tem de ser maior que zero, com no máximo 2 casas decimais";
                throw new RegraNegocioException($"A quantidade de {ingrediente.Nome} {regra}.");
            }
        }
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
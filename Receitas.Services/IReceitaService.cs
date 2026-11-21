using Receitas.Models.Dtos;

namespace Receitas.Services;

public interface IReceitaService
{
    Task<List<ReceitaDto>> ObterTodasAsync();
    Task<ReceitaDto?> ObterPorIdAsync(int id);
    Task<ReceitaDto> CriarAsync(ReceitaGuardarDto dto, int autorId);
    Task<ResultadoOperacao> AtualizarAsync(int id, ReceitaGuardarDto dto, int utilizadorId, bool eAdmin);
    Task<ResultadoOperacao> ApagarAsync(int id, int utilizadorId, bool eAdmin);
}
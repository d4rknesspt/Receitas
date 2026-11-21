using Receitas.Models.Dtos;

namespace Receitas.Services;

public interface IReceitaService
{
    Task<List<ReceitaDto>> ObterTodasAsync(int? utilizadorId, bool eAdmin);
    Task<ReceitaDto?> ObterPorIdAsync(int id, int? utilizadorId, bool eAdmin);
    Task<ReceitaDto> CriarAsync(ReceitaGuardarDto dto, int autorId);
    Task<ResultadoOperacao> AtualizarAsync(int id, ReceitaGuardarDto dto, int utilizadorId, bool eAdmin);
    Task<ResultadoOperacao> ApagarAsync(int id, int utilizadorId, bool eAdmin);
    Task<bool> ValidarAsync(int id);
}
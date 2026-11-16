using Receitas.Models.Dtos;

namespace Receitas.Services;

public interface IReceitaService
{
    Task<List<ReceitaDto>> ObterTodasAsync();
    Task<ReceitaDto?> ObterPorIdAsync(int id);
    Task<ReceitaDto> CriarAsync(ReceitaGuardarDto dto);
    Task<bool> AtualizarAsync(int id, ReceitaGuardarDto dto);
    Task<bool> ApagarAsync(int id);
}
using Receitas.Models.Dtos;

namespace Receitas.Services;

public interface IUtilizadorService
{
    Task<List<UtilizadorDto>> ObterTodosAsync();
    Task<ResultadoOperacao> AlterarBloqueioAsync(int id, bool bloqueado, int adminId);
}
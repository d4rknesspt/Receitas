using Receitas.DAL.Repositories;
using Receitas.Models;
using Receitas.Models.Dtos;

namespace Receitas.Services;

public class UtilizadorService : IUtilizadorService
{
    private readonly IUtilizadorRepository _repositorio;

    public UtilizadorService(IUtilizadorRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<List<UtilizadorDto>> ObterTodosAsync()
    {
        var utilizadores = await _repositorio.ObterTodosAsync();

        return utilizadores.Select(u => new UtilizadorDto
        {
            Id = u.Id,
            Nome = u.Nome,
            Email = u.Email,
            Perfil = u.Perfil.ToString(),
            Bloqueado = u.Bloqueado
        }).ToList();
    }

    public async Task<ResultadoOperacao> AlterarBloqueioAsync(int id, bool bloqueado, int adminId)
    {
        var utilizador = await _repositorio.ObterPorIdAsync(id);

        if (utilizador == null)
        {
            return ResultadoOperacao.NaoEncontrada;
        }

        if (utilizador.Id == adminId || utilizador.Perfil == Perfil.Administrador)
        {
            return ResultadoOperacao.SemPermissao;
        }

        await _repositorio.DefinirBloqueioAsync(id, bloqueado);
        return ResultadoOperacao.Sucesso;
    }
}
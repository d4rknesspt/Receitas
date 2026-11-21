using Microsoft.EntityFrameworkCore;
using Receitas.Models;

namespace Receitas.DAL.Repositories;

public class UtilizadorRepository : IUtilizadorRepository
{
    private readonly ReceitasDbContext _db;

    public UtilizadorRepository(ReceitasDbContext db)
    {
        _db = db;
    }

    public async Task<Utilizador?> ObterPorEmailAsync(string email)
    {
        return await _db.Utilizadores
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> EmailExisteAsync(string email)
    {
        return await _db.Utilizadores.AnyAsync(u => u.Email == email);
    }

    public async Task<Utilizador> CriarAsync(Utilizador utilizador)
    {
        _db.Utilizadores.Add(utilizador);
        await _db.SaveChangesAsync();
        return utilizador;
    }

    public async Task<List<Utilizador>> ObterTodosAsync()
    {
        return await _db.Utilizadores
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    public async Task<Utilizador?> ObterPorIdAsync(int id)
    {
        return await _db.Utilizadores.FindAsync(id);
    }

    public async Task<bool> DefinirBloqueioAsync(int id, bool bloqueado)
    {
        var utilizador = await _db.Utilizadores.FindAsync(id);

        if (utilizador == null)
        {
            return false;
        }

        utilizador.Bloqueado = bloqueado;
        await _db.SaveChangesAsync();
        return true;
    }
}
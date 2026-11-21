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
}
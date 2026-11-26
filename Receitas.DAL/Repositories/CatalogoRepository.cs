using Microsoft.EntityFrameworkCore;
using Receitas.Models;

namespace Receitas.DAL.Repositories;

public class CatalogoRepository : ICatalogoRepository
{
    private readonly ReceitasDbContext _db;

    public CatalogoRepository(ReceitasDbContext db)
    {
        _db = db;
    }

    public async Task<List<Categoria>> ObterCategoriasAsync()
    {
        return await _db.Categorias
            .OrderBy(c => c.Nome)
            .ToListAsync();
    }

    public async Task<List<Ingrediente>> ObterIngredientesValidadosAsync()
    {
        return await _db.Ingredientes
            .Where(i => i.Validado)
            .OrderBy(i => i.Nome)
            .ToListAsync();
    }

    public async Task<List<Ingrediente>> ObterIngredientesPendentesAsync()
    {
        return await _db.Ingredientes
            .Where(i => !i.Validado)
            .OrderBy(i => i.Nome)
            .ToListAsync();
    }

    public async Task<List<Ingrediente>> ObterIngredientesPorIdsAsync(List<int> ids)
    {
        return await _db.Ingredientes
            .Where(i => ids.Contains(i.Id))
            .ToListAsync();
    }

    public async Task<Ingrediente?> ObterIngredientePorIdAsync(int id)
    {
        return await _db.Ingredientes.FindAsync(id);
    }

    public async Task<bool> IngredienteExisteAsync(string nome, int? ignorarId = null)
    {
        return await _db.Ingredientes.AnyAsync(i =>
            i.Nome == nome && (ignorarId == null || i.Id != ignorarId));
    }

    public async Task<Ingrediente> CriarIngredienteAsync(Ingrediente ingrediente)
    {
        _db.Ingredientes.Add(ingrediente);
        await _db.SaveChangesAsync();
        return ingrediente;
    }

    public async Task AtualizarIngredienteAsync(Ingrediente ingrediente)
    {
        _db.Ingredientes.Update(ingrediente);
        await _db.SaveChangesAsync();
    }

    public async Task ApagarIngredienteAsync(Ingrediente ingrediente)
    {
        _db.Ingredientes.Remove(ingrediente);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> ValidarIngredienteAsync(int id)
    {
        var ingrediente = await _db.Ingredientes.FindAsync(id);

        if (ingrediente == null)
        {
            return false;
        }

        ingrediente.Validado = true;
        await _db.SaveChangesAsync();
        return true;
    }
}
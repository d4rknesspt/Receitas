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
}
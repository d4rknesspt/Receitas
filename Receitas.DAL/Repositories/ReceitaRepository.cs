using Microsoft.EntityFrameworkCore;
using Receitas.Models;

namespace Receitas.DAL.Repositories;

public class ReceitaRepository : IReceitaRepository
{
    private readonly ReceitasDbContext _db;

    public ReceitaRepository(ReceitasDbContext db)
    {
        _db = db;
    }

    public async Task<List<Receita>> ObterTodasAsync()
    {
        return await _db.Receitas
            .Include(r => r.Categoria)
            .Include(r => r.Ingredientes)
                .ThenInclude(ri => ri.Ingrediente)
            .ToListAsync();
    }

    public async Task<Receita?> ObterPorIdAsync(int id)
    {
        return await _db.Receitas
            .Include(r => r.Categoria)
            .Include(r => r.Ingredientes)
                .ThenInclude(ri => ri.Ingrediente)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Receita> CriarAsync(Receita receita)
    {
        _db.Receitas.Add(receita);
        await _db.SaveChangesAsync();
        return receita;
    }

    public async Task<bool> AtualizarAsync(Receita receita)
    {
        var existente = await _db.Receitas
            .Include(r => r.Ingredientes)
            .FirstOrDefaultAsync(r => r.Id == receita.Id);

        if (existente == null)
        {
            return false;
        }

        existente.Titulo = receita.Titulo;
        existente.ModoPreparacao = receita.ModoPreparacao;
        existente.CategoriaId = receita.CategoriaId;
        existente.Dificuldade = receita.Dificuldade;
        existente.DuracaoMinutos = receita.DuracaoMinutos;
        existente.Validada = receita.Validada;

        existente.Ingredientes.Clear();
        foreach (var ri in receita.Ingredientes)
        {
            existente.Ingredientes.Add(new ReceitaIngrediente
            {
                IngredienteId = ri.IngredienteId,
                Quantidade = ri.Quantidade,
                Unidade = ri.Unidade
            });
        }

        await _db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DefinirValidacaoAsync(int id, bool validada)
    {
        var receita = await _db.Receitas.FindAsync(id);

        if (receita == null)
        {
            return false;
        }

        receita.Validada = validada;
        await _db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> ApagarAsync(int id)
    {
        var receita = await _db.Receitas.FindAsync(id);

        if (receita == null)
        {
            return false;
        }

        _db.Receitas.Remove(receita);
        await _db.SaveChangesAsync();
        return true;
    }
}
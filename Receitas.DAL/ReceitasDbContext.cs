using Microsoft.EntityFrameworkCore;
using Receitas.Models;

namespace Receitas.DAL;

public class ReceitasDbContext : DbContext
{
    public ReceitasDbContext(DbContextOptions<ReceitasDbContext> options)
        : base(options)
    {
    }

    public DbSet<Receita> Receitas => Set<Receita>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Ingrediente> Ingredientes => Set<Ingrediente>();
    public DbSet<ReceitaIngrediente> ReceitaIngredientes => Set<ReceitaIngrediente>();
    public DbSet<Utilizador> Utilizadores => Set<Utilizador>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReceitaIngrediente>()
            .Property(ri => ri.Quantidade)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Utilizador>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
namespace Receitas.Models;

public class Receita
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string ModoPreparacao { get; set; } = string.Empty;

    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;

    public Dificuldade Dificuldade { get; set; }
    public int DuracaoMinutos { get; set; }

    public bool Validada { get; set; }

    public List<ReceitaIngrediente> Ingredientes { get; set; } = new();
}
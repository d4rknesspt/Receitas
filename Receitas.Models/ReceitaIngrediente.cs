namespace Receitas.Models;

public class ReceitaIngrediente
{
    public int Id { get; set; }

    public int ReceitaId { get; set; }
    public Receita Receita { get; set; } = null!;

    public int IngredienteId { get; set; }
    public Ingrediente Ingrediente { get; set; } = null!;

    public decimal Quantidade { get; set; }
    public string Unidade { get; set; } = string.Empty;
}
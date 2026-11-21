namespace Receitas.Models.Dtos;

public class FiltroReceitasDto
{
    public string? Texto { get; set; }
    public int? CategoriaId { get; set; }
    public Dificuldade? Dificuldade { get; set; }
    public int? DuracaoMax { get; set; }
    public string? Ingrediente { get; set; }
}
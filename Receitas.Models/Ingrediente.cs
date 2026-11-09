namespace Receitas.Models;

public class Ingrediente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Validado { get; set; }
}
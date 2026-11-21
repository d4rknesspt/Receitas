using System.ComponentModel.DataAnnotations;

namespace Receitas.Models.Dtos;

public class CategoriaDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}

public class IngredienteDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}

public class IngredienteCriarDto
{
    [Required, StringLength(80, MinimumLength = 2)]
    public string Nome { get; set; } = string.Empty;
}

public class IngredienteGestaoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Validado { get; set; }
}
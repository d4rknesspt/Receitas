using System.ComponentModel.DataAnnotations;

namespace Receitas.Models.Dtos;

// O que a API DEVOLVE
public class ReceitaDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string ModoPreparacao { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public Dificuldade Dificuldade { get; set; }
    public int DuracaoMinutos { get; set; }
    public bool Validada { get; set; }
    public List<ReceitaIngredienteDto> Ingredientes { get; set; } = new();
}

public class ReceitaIngredienteDto
{
    public int IngredienteId { get; set; }
    public string Ingrediente { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public string Unidade { get; set; } = string.Empty;
}

// O que a API RECEBE ao criar ou atualizar
public class ReceitaGuardarDto
{
    [Required, StringLength(150, MinimumLength = 3)]
    public string Titulo { get; set; } = string.Empty;

    [Required, MinLength(10)]
    public string ModoPreparacao { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int CategoriaId { get; set; }

    public Dificuldade Dificuldade { get; set; }

    [Range(1, 1440)]
    public int DuracaoMinutos { get; set; }

    [MinLength(1)]
    public List<ReceitaIngredienteGuardarDto> Ingredientes { get; set; } = new();
}

public class ReceitaIngredienteGuardarDto
{
    [Range(1, int.MaxValue)]
    public int IngredienteId { get; set; }

    [Range(0.01, 100000)]
    public decimal Quantidade { get; set; }

    [Required, StringLength(30)]
    public string Unidade { get; set; } = string.Empty;
}
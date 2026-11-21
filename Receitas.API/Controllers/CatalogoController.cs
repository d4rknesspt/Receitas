using Microsoft.AspNetCore.Mvc;
using Receitas.Models.Dtos;
using Receitas.Services;

namespace Receitas.API.Controllers;

[ApiController]
[Route("api")]
public class CatalogoController : ControllerBase
{
    private readonly ICatalogoService _servico;

    public CatalogoController(ICatalogoService servico)
    {
        _servico = servico;
    }

    [HttpGet("categorias")]
    public async Task<ActionResult<List<CategoriaDto>>> ObterCategorias()
    {
        return Ok(await _servico.ObterCategoriasAsync());
    }

    [HttpGet("ingredientes")]
    public async Task<ActionResult<List<IngredienteDto>>> ObterIngredientes()
    {
        return Ok(await _servico.ObterIngredientesAsync());
    }
}
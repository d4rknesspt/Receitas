using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpPost("ingredientes")]
    public async Task<ActionResult<IngredienteGestaoDto>> CriarIngrediente(IngredienteCriarDto dto)
    {
        var criado = await _servico.CriarIngredienteAsync(dto, User.IsInRole("Administrador"));

        if (criado == null)
        {
            return Conflict(new { mensagem = "Esse ingrediente já existe." });
        }

        return Created("/api/ingredientes", criado);
    }

    [Authorize(Roles = "Administrador")]
    [HttpGet("ingredientes/pendentes")]
    public async Task<ActionResult<List<IngredienteGestaoDto>>> ObterPendentes()
    {
        return Ok(await _servico.ObterIngredientesPendentesAsync());
    }

    [Authorize(Roles = "Administrador")]
    [HttpPut("ingredientes/{id}/validar")]
    public async Task<IActionResult> ValidarIngrediente(int id)
    {
        var validado = await _servico.ValidarIngredienteAsync(id);
        return validado ? NoContent() : NotFound();
    }
}
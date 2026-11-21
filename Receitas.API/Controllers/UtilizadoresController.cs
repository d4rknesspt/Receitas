using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Receitas.Models.Dtos;
using Receitas.Services;

namespace Receitas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class UtilizadoresController : ControllerBase
{
    private readonly IUtilizadorService _servico;

    public UtilizadoresController(IUtilizadorService servico)
    {
        _servico = servico;
    }

    private int AdminId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<ActionResult<List<UtilizadorDto>>> ObterTodos()
    {
        return Ok(await _servico.ObterTodosAsync());
    }

    [HttpPut("{id}/bloquear")]
    public async Task<IActionResult> Bloquear(int id)
    {
        var resultado = await _servico.AlterarBloqueioAsync(id, true, AdminId);
        return TraduzirResultado(resultado);
    }

    [HttpPut("{id}/desbloquear")]
    public async Task<IActionResult> Desbloquear(int id)
    {
        var resultado = await _servico.AlterarBloqueioAsync(id, false, AdminId);
        return TraduzirResultado(resultado);
    }

    private IActionResult TraduzirResultado(ResultadoOperacao resultado)
    {
        return resultado switch
        {
            ResultadoOperacao.Sucesso => NoContent(),
            ResultadoOperacao.SemPermissao => Forbid(),
            _ => NotFound()
        };
    }
}
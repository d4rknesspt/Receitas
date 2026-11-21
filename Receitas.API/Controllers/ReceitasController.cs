using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Receitas.Models.Dtos;
using Receitas.Services;

namespace Receitas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReceitasController : ControllerBase
{
    private readonly IReceitaService _servico;

    public ReceitasController(IReceitaService servico)
    {
        _servico = servico;
    }

    private int UtilizadorId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private bool EAdmin => User.IsInRole("Administrador");

    [HttpGet]
    public async Task<ActionResult<List<ReceitaDto>>> ObterTodas()
    {
        return Ok(await _servico.ObterTodasAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReceitaDto>> ObterPorId(int id)
    {
        var receita = await _servico.ObterPorIdAsync(id);
        return receita == null ? NotFound() : Ok(receita);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ReceitaDto>> Criar(ReceitaGuardarDto dto)
    {
        var criada = await _servico.CriarAsync(dto, UtilizadorId);
        return CreatedAtAction(nameof(ObterPorId), new { id = criada.Id }, criada);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, ReceitaGuardarDto dto)
    {
        var resultado = await _servico.AtualizarAsync(id, dto, UtilizadorId, EAdmin);
        return TraduzirResultado(resultado);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Apagar(int id)
    {
        var resultado = await _servico.ApagarAsync(id, UtilizadorId, EAdmin);
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
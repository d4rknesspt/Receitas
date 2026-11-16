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

    [HttpPost]
    public async Task<ActionResult<ReceitaDto>> Criar(ReceitaGuardarDto dto)
    {
        var criada = await _servico.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = criada.Id }, criada);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, ReceitaGuardarDto dto)
    {
        var atualizada = await _servico.AtualizarAsync(id, dto);
        return atualizada ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Apagar(int id)
    {
        var apagada = await _servico.ApagarAsync(id);
        return apagada ? NoContent() : NotFound();
    }
}
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Receitas.Models.Dtos;
using Receitas.Services;

namespace Receitas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("registo")]
    public async Task<ActionResult<AuthRespostaDto>> Registar(RegistoDto dto)
    {
        var resposta = await _auth.RegistarAsync(dto);

        if (resposta == null)
        {
            return Conflict(new { mensagem = "Já existe uma conta com esse email." });
        }

        return Ok(resposta);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthRespostaDto>> Login(LoginDto dto)
    {
        var resposta = await _auth.LoginAsync(dto);

        if (resposta == null)
        {
            return Unauthorized(new { mensagem = "Email ou palavra-passe inválidos, ou conta bloqueada." });
        }

        return Ok(resposta);
    }

    [Authorize]
    [HttpGet("eu")]
    public IActionResult Eu()
    {
        return Ok(new
        {
            id = User.FindFirstValue(ClaimTypes.NameIdentifier),
            nome = User.FindFirstValue(ClaimTypes.Name),
            email = User.FindFirstValue(ClaimTypes.Email),
            perfil = User.FindFirstValue(ClaimTypes.Role)
        });
    }
}
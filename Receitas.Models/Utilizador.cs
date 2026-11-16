namespace Receitas.Models;

public class Utilizador
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Perfil Perfil { get; set; } = Perfil.Utilizador;
    public bool Bloqueado { get; set; }
}
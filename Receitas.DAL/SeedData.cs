using Microsoft.AspNetCore.Identity;
using Receitas.Models;

namespace Receitas.DAL;

public static class SeedData
{
    public static void Inicializar(ReceitasDbContext db)
    {
        if (db.Categorias.Any())
        {
            return;
        }

        var hasher = new PasswordHasher<Utilizador>();

        var admin = new Utilizador
        {
            Nome = "Administrador",
            Email = "admin@receitas.pt",
            Perfil = Perfil.Administrador
        };
        admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

        var filipe = new Utilizador
        {
            Nome = "Filipe",
            Email = "filipe@receitas.pt",
            Perfil = Perfil.Utilizador
        };
        filipe.PasswordHash = hasher.HashPassword(filipe, "Formador123!");

        var teste = new Utilizador
        {
            Nome = "Utilizador Teste",
            Email = "utilizador@receitas.pt",
            Perfil = Perfil.Utilizador
        };
        teste.PasswordHash = hasher.HashPassword(teste, "Teste123!");

        var sobremesa = new Categoria { Nome = "Sobremesa" };
        var pratoPrincipal = new Categoria { Nome = "Prato principal" };
        var sopa = new Categoria { Nome = "Sopa" };

        var farinha = new Ingrediente { Nome = "Farinha", Validado = true, TipoMedida = TipoMedida.Peso };
        var ovos = new Ingrediente { Nome = "Ovos", Validado = true, TipoMedida = TipoMedida.Unidade };
        var leite = new Ingrediente { Nome = "Leite", Validado = true, TipoMedida = TipoMedida.Volume };
        var acucar = new Ingrediente { Nome = "Açúcar", Validado = true, TipoMedida = TipoMedida.Peso };
        var batata = new Ingrediente { Nome = "Batata", Validado = true, TipoMedida = TipoMedida.Peso };
        var cebola = new Ingrediente { Nome = "Cebola", Validado = true, TipoMedida = TipoMedida.Unidade };
        var arroz = new Ingrediente { Nome = "Arroz", Validado = true, TipoMedida = TipoMedida.Peso };
        var frango = new Ingrediente { Nome = "Frango", Validado = true, TipoMedida = TipoMedida.Peso };

        var panquecas = new Receita
        {
            Titulo = "Panquecas",
            ModoPreparacao = "Misture a farinha, os ovos, o leite e o açúcar até obter uma massa lisa. Leve a uma frigideira antiaderente e vire quando surgirem bolhas.",
            Categoria = sobremesa,
            Dificuldade = Dificuldade.Facil,
            DuracaoMinutos = 20,
            Validada = true,
            Autor = admin,
            Ingredientes = new List<ReceitaIngrediente>
            {
                new() { Ingrediente = farinha, Quantidade = 200, Unidade = "g" },
                new() { Ingrediente = ovos, Quantidade = 2, Unidade = "unidade" },
                new() { Ingrediente = leite, Quantidade = 300, Unidade = "ml" },
                new() { Ingrediente = acucar, Quantidade = 15, Unidade = "g" }
            }
        };

        var sopaLegumes = new Receita
        {
            Titulo = "Sopa de legumes",
            ModoPreparacao = "Coza a batata e a cebola em água com sal. Triture tudo e sirva quente.",
            Categoria = sopa,
            Dificuldade = Dificuldade.Facil,
            DuracaoMinutos = 35,
            Validada = true,
            Autor = admin,
            Ingredientes = new List<ReceitaIngrediente>
            {
                new() { Ingrediente = batata, Quantidade = 500, Unidade = "g" },
                new() { Ingrediente = cebola, Quantidade = 1, Unidade = "unidade" }
            }
        };

        var arrozFrango = new Receita
        {
            Titulo = "Arroz de frango",
            ModoPreparacao = "Refogue a cebola, junte o frango em pedaços e deixe cozinhar. Acrescente o arroz e água e cozinhe até ficar no ponto.",
            Categoria = pratoPrincipal,
            Dificuldade = Dificuldade.Media,
            DuracaoMinutos = 50,
            Validada = true,
            Autor = teste,
            Ingredientes = new List<ReceitaIngrediente>
            {
                new() { Ingrediente = frango, Quantidade = 500, Unidade = "g" },
                new() { Ingrediente = arroz, Quantidade = 250, Unidade = "g" },
                new() { Ingrediente = cebola, Quantidade = 1, Unidade = "unidade" }
            }
        };

        db.Utilizadores.Add(filipe);
        db.Receitas.AddRange(panquecas, sopaLegumes, arrozFrango);
        db.SaveChanges();
    }
}
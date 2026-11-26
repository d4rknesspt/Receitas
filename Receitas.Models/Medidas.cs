namespace Receitas.Models;

public static class Medidas
{
    public static readonly IReadOnlyDictionary<TipoMedida, string[]> UnidadesPermitidas =
        new Dictionary<TipoMedida, string[]>
        {
            [TipoMedida.Unidade] = new[] { "unidade" },
            [TipoMedida.Peso] = new[] { "g", "kg" },
            [TipoMedida.Volume] = new[] { "ml", "l" },
        };

    public static bool UnidadeValida(TipoMedida tipo, string unidade)
    {
        return UnidadesPermitidas[tipo].Contains(unidade);
    }

    public static bool QuantidadeValida(TipoMedida tipo, decimal quantidade)
    {
        if (quantidade <= 0 || quantidade != Math.Round(quantidade, 2))
        {
            return false;
        }

        return tipo != TipoMedida.Unidade || quantidade == decimal.Truncate(quantidade);
    }
}
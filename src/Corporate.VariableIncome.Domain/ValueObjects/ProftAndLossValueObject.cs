using Corporate.VariableIncome.Domain.Exceptions;
using System.Globalization;

namespace Corporate.VariableIncome.Domain.ValueObjects;

/*
 * Comentário de Intenção:
 * 
 * Este objeto de valor representa a variação percentual de lucro ou prejuízo (P&L) 
 * de uma posição em renda variável. Como este valor pode ser negativo ou positivo,
 * ele não possui restrições de faixa numérica, apenas encapsula e formata a leitura
 * para garantir consistência e apresentação amigável.
 * 
 * Exemplo: 0.87 → "87,00%" | -0.1543 → "-15,43%"
 */
public readonly struct ProftAndLossValueObject
{
    public bool IsValid { get; }
    private decimal? Value { get; }

    private ProftAndLossValueObject(bool isValid, decimal? value)
    {
        IsValid = isValid;
        Value = value;
    }

    internal static ProftAndLossValueObject UnsafeBuildFromTrustedDatabaseRecord(decimal value)
        => new ProftAndLossValueObject(true, value);

    public static ProftAndLossValueObject Build(decimal value)
        => new ProftAndLossValueObject(true, value);

    public decimal GetValue()
    {
        ValueObjectException.ThrowValueObjectExceptionWhenResourceIsNotValid<ProftAndLossValueObject>(IsValid);
        return Value!.Value;
    }

    public string GetFormattedPercentage()
    {
        var percentual = GetValue() * 100;
        return percentual.ToString("N2", new CultureInfo("pt-BR")) + "%";
    }

    public static implicit operator ProftAndLossValueObject(decimal value)
        => Build(value);

    public static implicit operator decimal(ProftAndLossValueObject obj)
        => obj.GetValue();
}
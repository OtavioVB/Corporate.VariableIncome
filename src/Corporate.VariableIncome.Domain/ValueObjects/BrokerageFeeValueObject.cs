using Corporate.VariableIncome.Domain.Exceptions;
using System.Globalization;

namespace Corporate.VariableIncome.Domain.ValueObjects;

/*
 * Comentário de Intenção:
 * 
 * Este objeto de valor representa a taxa de corretagem aplicada a uma operação no domínio da corretora.
 * Sua função é garantir que o valor esteja entre 0 e 1 (inclusive), representando corretamente uma taxa percentual.
 * 
 * Ele é projetado como struct imutável e sua construção ocorre apenas via método `Build`, que aplica as regras de negócio
 * do domínio. Esse padrão evita a propagação de valores inválidos durante o processamento.
 */
public readonly struct BrokerageFeeValueObject
{
    public bool IsValid { get; }
    private decimal? Value { get; }

    /*
     * Comentário de Intenção:
     * 
     * A constante "MAX_PERCENTUAL" define o limite máximo da taxa de corretagem (1 = 100%)
     * A constante "MIN_PERCENTUAL" define o limite inferior (> 0)
     * Elas são públicas para facilitar validação em outras camadas e garantir consistência.
     */
    public const decimal MAX_PERCENTUAL = 1.0m;
    public const decimal MIN_PERCENTUAL = 0.0000001m; // não pode ser zero

    private BrokerageFeeValueObject(bool isValid, decimal? value)
    {
        IsValid = isValid;
        Value = value;
    }

    internal static BrokerageFeeValueObject UnsafeBuildFromTrustedDatabaseRecord(decimal input)
        => new BrokerageFeeValueObject(true, input);

    /*
     * Comentário de Intenção:
     * 
     * Validações aplicadas:
     * - O valor deve estar estritamente entre 0 e 1 (percentual)
     */
    public static BrokerageFeeValueObject Build(decimal value)
    {
        if (value < MIN_PERCENTUAL || value > MAX_PERCENTUAL)
            return new BrokerageFeeValueObject(false, null);

        return new BrokerageFeeValueObject(true, value);
    }

    public decimal GetValue()
    {
        ValueObjectException.ThrowValueObjectExceptionWhenResourceIsNotValid<BrokerageFeeValueObject>(IsValid);

        return Value!.Value;
    }

    /*
     * Comentário de Intenção:
     * 
     * Este método retorna o valor formatado como uma string percentual amigável,
     * por exemplo: 0.89 => "89%"
     */
    public string GetValueAsPercentageString()
    {
        var raw = GetValue();
        var percent = raw * 100;
        return $"{percent.ToString("0.##", CultureInfo.InvariantCulture)}%";
    }

    public static implicit operator BrokerageFeeValueObject(decimal value)
        => Build(value);

    public static implicit operator decimal(BrokerageFeeValueObject obj)
        => obj.GetValue();
}
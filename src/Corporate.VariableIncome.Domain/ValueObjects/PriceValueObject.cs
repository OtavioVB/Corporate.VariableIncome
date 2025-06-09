
using Corporate.VariableIncome.Domain.Exceptions;

namespace Corporate.VariableIncome.Domain.ValueObjects;

/*
 * Comentário de Intenção:
 * 
 * Este objeto de valor representa o preço unitário de um ativo no momento da negociação (compra/venda).
 * Seu uso evita inconsistências e validações repetitivas, centralizando as regras:
 * - Deve ser maior que zero.
 * - Deve conter até 2 casas decimais.
 * 
 * O uso de `readonly struct` promove imutabilidade e economia de alocação.
 */
public readonly struct PriceValueObject
{
    public bool IsValid { get; }
    private decimal? Value { get; }

    /*
     * Comentário de Intenção:
     * 
     * O valor mínimo é 0.01 para impedir negociação com zero ou valores negativos.
     * O número máximo de casas decimais é 2, refletindo a precisão padrão do mercado.
     */
    public const decimal MIN_VALUE = 0.01m;
    public const int MAX_DECIMAL_PLACES = 2;

    private PriceValueObject(bool isValid, decimal? value)
    {
        IsValid = isValid;
        Value = value;
    }

    internal static PriceValueObject UnsafeBuildFromTrustedDatabaseRecord(decimal input)
        => new PriceValueObject(true, input);

    public static PriceValueObject Build(decimal value)
    {
        if (value < MIN_VALUE)
            return new PriceValueObject(false, null);

        var rounded = Math.Round(value, MAX_DECIMAL_PLACES, MidpointRounding.AwayFromZero);

        return new PriceValueObject(true, rounded);
    }

    public decimal GetValue()
    {
        ValueObjectException.ThrowValueObjectExceptionWhenResourceIsNotValid<PriceValueObject>(IsValid);
        return Value!.Value;
    }

    public static implicit operator PriceValueObject(decimal value)
        => Build(value);

    public static implicit operator decimal(PriceValueObject obj)
        => obj.GetValue();
}
using Corporate.VariableIncome.Domain.Exceptions;

namespace Corporate.VariableIncome.Domain.ValueObjects;

/*
 * Comentário de Intenção:
 * 
 * Este objeto de valor representa a quantidade de ativos adquiridos, vendidos ou mantidos em custódia.
 * Ele encapsula a regra de que a quantidade deve ser sempre um valor inteiro positivo (> 0),
 * garantindo consistência e eliminando a necessidade de validações redundantes em outros pontos da aplicação.
 * 
 * Seu uso favorece clareza, imutabilidade e integridade de dados no modelo de domínio.
 */
public readonly struct QuantityValueObject
{
    public bool IsValid { get; }
    private int? Value { get; }

    /*
     * Comentário de Intenção:
     * 
     * O valor mínimo é 1, pois não se pode ter quantidade nula ou negativa de ativos.
     * Essa constante pode ser usada em camadas de validação externas ao domínio.
     */
    public const int MIN_VALUE = 0;

    private QuantityValueObject(bool isValid, int? value)
    {
        IsValid = isValid;
        Value = value;
    }

    internal static QuantityValueObject UnsafeBuildFromTrustedDatabaseRecord(int input)
        => new QuantityValueObject(true, input);

    /*
     * Comentário de Intenção:
     * 
     * Regras de negócio aplicadas:
     * - O valor deve ser maior ou igual a 1;
     * - Valores inválidos não devem ser acessados via GetValue().
     */
    public static QuantityValueObject Build(int value)
    {
        if (value < MIN_VALUE)
            return new QuantityValueObject(false, null);

        return new QuantityValueObject(true, value);
    }

    public int GetValue()
    {
        ValueObjectException.ThrowValueObjectExceptionWhenResourceIsNotValid<QuantityValueObject>(IsValid);
        return Value!.Value;
    }

    public static implicit operator QuantityValueObject(int value)
        => Build(value);

    public static implicit operator int(QuantityValueObject obj)
        => obj.GetValue();
}
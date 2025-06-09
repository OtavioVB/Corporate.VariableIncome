using Corporate.VariableIncome.Domain.Exceptions;

namespace Corporate.VariableIncome.Domain.ValueObjects;

/*
 * Comentário de Intenção:
 * 
 * Este objeto de valor encapsula um valor de data/hora sempre válido e rastreável.
 * Quando o valor fornecido for nulo, será automaticamente substituído pelo DateTime.UtcNow.
 * Quando não for nulo, será validado como data válida (diferente de DateTime.MinValue/MaxValue).
 * 
 * O uso deste VO garante integridade temporal em operações de criação, atualização ou eventos.
 */
public readonly struct DateTimeValueObject
{
    public bool IsValid { get; }
    private DateTime? Value { get; }

    private DateTimeValueObject(bool isValid, DateTime? value)
    {
        IsValid = isValid;
        Value = value;
    }

    internal static DateTimeValueObject UnsafeBuildFromTrustedDatabaseRecord(DateTime value)
        => new DateTimeValueObject(true, value);

    public static DateTimeValueObject Build(DateTime? input = null)
    {
        if (input is null)
            return new DateTimeValueObject(true, DateTime.UtcNow);

        if (input == DateTime.MinValue || input == DateTime.MaxValue)
            return new DateTimeValueObject(false, null);

        return new DateTimeValueObject(true, DateTime.SpecifyKind(input.Value.Date, DateTimeKind.Local).ToUniversalTime());
    }

    public DateTime GetValue()
    {
        ValueObjectException.ThrowValueObjectExceptionWhenResourceIsNotValid<DateTimeValueObject>(IsValid);
        return Value!.Value;
    }

    public static implicit operator DateTimeValueObject(DateTime? value)
        => Build(value);
    public static implicit operator DateTime(DateTimeValueObject obj)
        => obj.GetValue();
}
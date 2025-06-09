using Corporate.VariableIncome.Domain.Exceptions;

namespace Corporate.VariableIncome.Domain.ValueObjects;

public readonly struct OffsetValueObject
{
    public bool IsValid { get; }
    private int? Value { get; }

    public const int DEFAULT_OFFSET = 25;
    public const int MIN_VALUE = 1;
    public const int MAX_VALUE = 50;

    private OffsetValueObject(bool isValid, int? value)
    {
        IsValid = isValid;
        Value = value;
    }

    public static OffsetValueObject Build(int? value = null)
    {
        var resolved = value ?? DEFAULT_OFFSET;

        if (resolved < MIN_VALUE || resolved > MAX_VALUE)
            return new OffsetValueObject(false, null);

        return new OffsetValueObject(true, resolved);
    }

    public int GetValue()
    {
        ValueObjectException.ThrowValueObjectExceptionWhenResourceIsNotValid<OffsetValueObject>(IsValid);
        return Value!.Value;
    }

    public string GetValueAsString()
        => GetValue().ToString();

    public static implicit operator OffsetValueObject(int value)
        => Build(value);
    public static implicit operator int(OffsetValueObject obj)
        => obj.GetValue();
}
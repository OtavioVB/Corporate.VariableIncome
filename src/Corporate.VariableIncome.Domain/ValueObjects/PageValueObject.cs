using Corporate.VariableIncome.Domain.Exceptions;

namespace Corporate.VariableIncome.Domain.ValueObjects;

public readonly struct PageValueObject
{
    public bool IsValid { get; }
    private int? Value { get; }

    public const int MIN_PAGE = 1;

    private PageValueObject(bool isValid, int? value)
    {
        IsValid = isValid;
        Value = value;
    }

    public static PageValueObject Build(int? value)
    {
        if (value is null || value < MIN_PAGE)
            return new PageValueObject(false, null);

        return new PageValueObject(true, value);
    }

    public int GetValue()
    {
        ValueObjectException.ThrowValueObjectExceptionWhenResourceIsNotValid<PageValueObject>(IsValid);
        return Value!.Value;
    }

    public int GetSkip(OffsetValueObject offset)
        => (GetValue() - 1) * offset;
    public string GetValueAsString()
        => GetValue().ToString();

    public static implicit operator PageValueObject(int obj)
        => Build(obj);

    public static implicit operator int(PageValueObject obj)
        => obj.GetValue();
}
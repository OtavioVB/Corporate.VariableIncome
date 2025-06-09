using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Enumerators;
using Corporate.VariableIncome.Domain.Exceptions;

namespace Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.ValueObjects;

/*
 * Comentário de Intenção:
 * 
 * Este objeto de valor representa o tipo de operação financeira (compra ou venda).
 * Sua função é encapsular o enum `EnumTypeOperation` e centralizar validações sobre ele.
 * A construção é controlada por dois métodos: um a partir de string e outro a partir do próprio enum.
 * 
 * Esse padrão permite integração segura com entrada de APIs e evita propagação de enums não validados.
 */
public readonly struct TypeOperationValueObject
{
    public bool IsValid { get; }
    private EnumTypeOperation? Value { get; }

    private TypeOperationValueObject(bool isValid, EnumTypeOperation? value)
    {
        IsValid = isValid;
        Value = value;
    }

    public static TypeOperationValueObject BUY => Build(EnumTypeOperation.BUY);
    public static TypeOperationValueObject SELL => Build(EnumTypeOperation.SELL);

    internal static TypeOperationValueObject UnsafeBuildFromTrustedDatabaseRecord(EnumTypeOperation value)
        => new TypeOperationValueObject(true, value);

    public static TypeOperationValueObject Build(EnumTypeOperation value)
    {
        if (!Enum.IsDefined(typeof(EnumTypeOperation), value))
            return new TypeOperationValueObject(false, null);

        return new TypeOperationValueObject(true, value);
    }

    public static TypeOperationValueObject Build(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new TypeOperationValueObject(false, null);

        var parsed = Enum.TryParse<EnumTypeOperation>(value.Trim(), ignoreCase: true, out var result);

        if (!parsed || !Enum.IsDefined(typeof(EnumTypeOperation), result))
            return new TypeOperationValueObject(false, null);

        return new TypeOperationValueObject(true, result);
    }

    public EnumTypeOperation GetValue()
    {
        ValueObjectException.ThrowValueObjectExceptionWhenResourceIsNotValid<TypeOperationValueObject>(IsValid);
        return Value!.Value;
    }

    public string GetValueAsString()
        => GetValue().ToString();

    public static implicit operator TypeOperationValueObject(string value)
        => Build(value);
    public static implicit operator TypeOperationValueObject(EnumTypeOperation value)
        => Build(value);
    public static implicit operator EnumTypeOperation(TypeOperationValueObject obj)
        => obj.GetValue();
    public static implicit operator string(TypeOperationValueObject obj)
        => obj.GetValueAsString();
}
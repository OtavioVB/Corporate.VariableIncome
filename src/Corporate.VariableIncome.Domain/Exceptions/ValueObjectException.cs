namespace Corporate.VariableIncome.Domain.Exceptions;

public sealed class ValueObjectException : Exception
{
    public ValueObjectException(string message) : base(message)
    {
    }

    public static void ThrowValueObjectExceptionWhenResourceIsNotValid<T>(bool isValid)
    {
        if (!isValid)
            throw new ValueObjectException($"Is not possible to access the resource {typeof(T)}, because is not valid.");
    }
}

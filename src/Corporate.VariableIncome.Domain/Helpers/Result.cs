using System.Text.Json.Serialization;

namespace Corporate.VariableIncome.Domain.Helpers;

public readonly struct Result
{
    public bool IsFailed { get; }

    [JsonIgnore]
    public bool IsSuccess => !IsFailed;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; }

    private Result(bool isFailed, string? message)
    {
        IsFailed = isFailed;
        Message = message;
    }


    public static Result Success(string? message = null)
        => new Result(
            isFailed: false,
            message: message);

    public static Result Error(string message)
        => new Result(
            isFailed: true,
            message: message);
    public string? GetMessage()
        => Message;

    public string GetRequiredMessage()
        => Message!;
}

public sealed class Result<T>
{
    public bool IsFailed { get; }

    [JsonIgnore]
    public bool IsSuccess => !IsFailed;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; }
    private T? Value { get; }

    private Result(bool isFailed, string? message, T? value)
    {
        IsFailed = isFailed;
        Message = message;
        Value = value;
    }

    public static Result<T> Success(T value, string? message = null)
        => new Result<T>(
            isFailed: false,
            message: message,
            value: value);

    public static Result<T> Error(string message)
        => new Result<T>(
            isFailed: true,
            message: message,
            value: default);

    public T GetValue()
        => IsSuccess ? Value! : throw new InvalidOperationException("Não é possível obter objeto de resultado de operação em estado inválido.");

    public string? GetMessage()
        => Message;

    public string GetRequiredMessage()
        => Message!;
}

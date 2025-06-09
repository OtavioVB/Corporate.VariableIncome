using Corporate.VariableIncome.Domain.Exceptions;

namespace Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.ValueObjects;

/*
 * Comentário de Intenção:
 * 
 * Este objeto de valor representa o código de negociação de um ativo (ex: ITSA3, PETR4, KNRI11).
 * Ele encapsula regras de validação e formatação para garantir que apenas códigos válidos e padronizados
 * trafeguem no domínio. A padronização em letras maiúsculas assegura consistência em buscas, persistência
 * e comparação entre domínios externos (ex: integrações com B3).
 * 
 * O uso de `readonly struct` garante imutabilidade e eficiência.
 */
public readonly struct AssetCodeValueObject
{
    public bool IsValid { get; }
    private string? Value { get; }

    /*
     * Comentário de Intenção:
     * 
     * A constante MAX_LENGTH foi definida com valor 10, pois ativos listados na B3 ou mercados internacionais
     * (ex: BDRs, ETFs, FIIs) não ultrapassam esse limite. A constante é pública para reuso em outras camadas.
     */
    public const int MAX_LENGTH = 10;

    private AssetCodeValueObject(bool isValid, string? value)
    {
        IsValid = isValid;
        Value = value;
    }

    internal static AssetCodeValueObject UnsafeBuildFromTrustedDatabaseRecord(string input)
        => new AssetCodeValueObject(
            isValid: true,
            value: input);

    /*
     * Comentário de Intenção:
     * 
     * Regras de negócio aplicadas no Build:
     * - Valor não pode ser nulo, vazio ou espaços em branco;
     * - Valor deve ter no máximo 10 caracteres;
     * - O valor armazenado será sempre em letras maiúsculas (padronização).
     */
    public static AssetCodeValueObject Build(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new AssetCodeValueObject(false, null);

        var trimmed = input.Trim();

        if (trimmed.Length > MAX_LENGTH)
            return new AssetCodeValueObject(false, null);

        var uppercased = trimmed.ToUpperInvariant();

        return new AssetCodeValueObject(true, uppercased);
    }

    public string GetValue()
    {
        ValueObjectException.ThrowValueObjectExceptionWhenResourceIsNotValid<AssetCodeValueObject>(IsValid);
        return Value!;
    }

    public static implicit operator AssetCodeValueObject(string input)
        => Build(input);

    public static implicit operator string(AssetCodeValueObject obj)
        => obj.GetValue();
}



using Corporate.VariableIncome.Domain.Exceptions;

namespace Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.ValueObjects;

/*
 * Comentário de Intenção:
 * 
 * Este objeto de valor representa o nome de um ativo no domínio de custódia e renda variável.
 * Sua responsabilidade é garantir que o nome seja válido, ou seja, não nulo, não em branco,
 * e com no máximo 32 caracteres. Isso previne inconsistências e propagação de dados inválidos
 * ao longo do ciclo de vida da aplicação.
 * 
 * O uso de struct readonly foi adotado para garantir imutabilidade e economia de alocação.
 */
public readonly struct AssetNameValueObject
{
    public bool IsValid { get; }
    private string? Value { get; }

    /*
     * Comentário de Intenção:
     * 
     * Esta constante define o tamanho máximo de um nome de ativo (ex: "Fundo XP Multimercado").
     * Foi definida como pública para ser usada em validações externas (DTOs, Swagger, ORM etc.).
     */
    public const int MAX_LENGTH = 32;

    private AssetNameValueObject(bool isValid, string? value)
    {
        IsValid = isValid;
        Value = value;
    }

    internal static AssetNameValueObject UnsafeBuildFromTrustedDatabaseRecord(string input)
        => new AssetNameValueObject(true, input);

    /*
     * Comentário de Intenção:
     * 
     * O método Build é a única via de construção pública deste objeto de valor, 
     * sendo responsável por aplicar as seguintes regras de negócio:
     * - Valor não pode ser nulo, vazio ou espaços em branco.
     * - Valor não pode exceder 32 caracteres.
     */
    public static AssetNameValueObject Build(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new AssetNameValueObject(false, null);

        if (name.Length > MAX_LENGTH)
            return new AssetNameValueObject(false, null);

        return new AssetNameValueObject(true, name.Trim());
    }

    public string GetValue()
    {
        ValueObjectException.ThrowValueObjectExceptionWhenResourceIsNotValid<AssetNameValueObject>(IsValid);

        return Value!;
    }

    public static implicit operator AssetNameValueObject(string name)
        => Build(name);

    public static implicit operator string(AssetNameValueObject obj)
        => obj.GetValue();
}

using System.Globalization;
using Corporate.VariableIncome.Domain.Exceptions;

namespace Corporate.VariableIncome.Domain.BoundedContexts.UserContext.ValueObjects;

/*
 * Comentário de Intenção:
 * 
 * Este objeto de valor representa o nome de usuário dentro do domínio da aplicação. O objetivo é encapsular a lógica
 * de validação e formatação de nomes de forma que somente valores válidos possam ser utilizados durante o processamento
 * do domínio.
 * 
 * Ele é declarado como 'readonly struct' para garantir imutabilidade e segurança de estado durante o ciclo de vida em memória.
 * A construção é controlada via método estático 'Build', que aplica regras de negócio e garante que o estado do objeto
 * só seja válido se todas as regras forem respeitadas.
 */
public readonly struct UserNameValueObject
{
    public bool IsValid { get; }
    private string? Value { get; }

    private UserNameValueObject(bool isValid, string? value)
    {
        IsValid = isValid;
        Value = value;
    }

    /*
     * Comentário de Intenção:
     * 
     * A constante "MAX_LENGTH" define o limite máximo de caracteres para o nome de usuário,
     * e foi exposta publicamente com o intuito de ser referenciada em diferentes camadas da aplicação,
     * como validação de entrada em APIs, mapeamento de colunas em bancos de dados ou geração de documentação
     * de contratos (ex: Swagger).
     * 
     * Isso evita duplicação de regras e mantém o domínio como única fonte de verdade.
     */
    public const int MAX_LENGTH = 64;

    /*
     * Comentário de Intenção:
     * 
     * A constante "CULTURE_INFO" define a cultura utilizada para aplicar a formatação de capitalização das palavras.
     * Foi declarada com escopo restrito, pois sua utilidade se dá apenas dentro da regra de negócio do próprio
     * objeto de valor.
     */
    private const string CULTURE_INFO = "pt-BR";

    internal static UserNameValueObject UnsafeBuildFromTrustedDatabaseRecord(string input)
        => new UserNameValueObject(
            isValid: true,
            value: input);

    public static UserNameValueObject Build(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new UserNameValueObject(
                isValid: false,
                value: null);

        if (name.Length > MAX_LENGTH)
            return new UserNameValueObject(
                isValid: false,
                value: null);

        var formatted = CultureInfo
            .GetCultureInfo(CULTURE_INFO)
            .TextInfo
            .ToTitleCase(name.Trim().ToLower());

        return new UserNameValueObject(
            isValid: true,
            value: formatted);
    }

    public string GetValue()
    {
        ValueObjectException.ThrowValueObjectExceptionWhenResourceIsNotValid<UserNameValueObject>(IsValid);

        return Value!;
    }

    public static implicit operator UserNameValueObject(string obj)
        => Build(obj);
    public static implicit operator string(UserNameValueObject obj)
        => obj.GetValue();
}


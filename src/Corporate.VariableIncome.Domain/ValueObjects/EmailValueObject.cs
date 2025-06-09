using Corporate.VariableIncome.Domain.Exceptions;
using System.Net.Mail;

namespace Corporate.VariableIncome.Domain.ValueObjects;

/*
 * Comentário de Intenção:
 * 
 * Este objeto de valor representa um endereço de e-mail válido no domínio da aplicação.
 * Ele encapsula as regras de negócio para validação e reutilização segura do atributo "Email",
 * garantindo que seu valor esteja sempre em conformidade com os padrões técnicos e funcionais exigidos.
 * 
 * O uso de struct readonly foi escolhido por garantir imutabilidade e eficiência de memória.
 * Sua construção é controlada via método Build, impedindo instanciamento direto com valores inválidos.
 */
public readonly struct EmailValueObject
{
    public bool IsValid { get; }
    private string? Value { get; }

    /*
     * Comentário de Intenção:
     * 
     * A constante "MAX_LENGTH" define o limite técnico e de domínio para o e-mail,
     * sendo 255 o padrão adotado por serviços de autenticação e bancos de dados (ex: VARCHAR(255)).
     * Essa constante foi exposta publicamente para servir como referência única e centralizada
     * em múltiplas camadas da aplicação.
     */
    public const int MAX_LENGTH = 255;

    private EmailValueObject(bool isValid, string? value)
    {
        IsValid = isValid;
        Value = value;
    }

    internal static EmailValueObject UnsafeBuildFromTrustedDatabaseRecord(string input)
        => new EmailValueObject(
            isValid: true,
            value: input);

    /*
     * Comentário de Intenção:
     * 
     * O método Build encapsula as regras de validação para um endereço de e-mail:
     * - Não pode ser nulo ou vazio;
     * - Deve respeitar o limite de tamanho;
     * - Deve estar em um formato válido (validado via MailAddress).
     * 
     * Essa abordagem garante que qualquer instância válida de EmailValueObject seja segura
     * para ser usada no domínio sem necessidade de validações adicionais.
     */
    public static EmailValueObject Build(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return new EmailValueObject(false, null);

        if (email.Length > MAX_LENGTH)
            return new EmailValueObject(false, null);

        try
        {
            var mail = new MailAddress(email);
        }
        catch
        {
            return new EmailValueObject(false, null);
        }

        return new EmailValueObject(true, email.Trim().ToLower());
    }

    public string GetValue()
    {
        ValueObjectException.ThrowValueObjectExceptionWhenResourceIsNotValid<EmailValueObject>(IsValid);

        return Value!;
    }

    public static implicit operator EmailValueObject(string email)
        => Build(email);

    public static implicit operator string(EmailValueObject obj)
        => obj.GetValue();
}

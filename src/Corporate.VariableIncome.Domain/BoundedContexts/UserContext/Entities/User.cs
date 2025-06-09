using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.PositionContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.UserContext.ValueObjects;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Domain.BoundedContexts.UserContext.Entities;

/*
 * Comentário de Intenção:
 * 
 * Essa é a entidade de negócio designada por encapsular o domínio de usuário dentro da corretora. Nela contém atributos que encapsulam
 * as regras de negócio necessária para a criação de cada um.
 */
public sealed class User
{
    public User(IdValueObject id, UserNameValueObject name, BrokerageFeeValueObject brokerageFee, EmailValueObject email)
    {
        Id = id;
        Name = name;
        BrokerageFee = brokerageFee;
        Email = email;
    }

    public IdValueObject Id { get; set; }
    public UserNameValueObject Name { get; set; }
    public BrokerageFeeValueObject BrokerageFee { get; set; }
    public EmailValueObject Email { get; set; }

    public bool IsValid => Id.IsValid && Name.IsValid && BrokerageFee.IsValid && Email.IsValid;

    public IList<Operation>? Operations { get; set; } = null;
    public IList<PositionSnapshot>? PositionSnapshots { get; set; } = null;
}

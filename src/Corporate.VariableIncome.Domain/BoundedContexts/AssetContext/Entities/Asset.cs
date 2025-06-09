using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.ValueObjects;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.PositionContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.QuotationContext.Entities;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.Entities;

/*
 * Comentário de Intenção:
 * 
 * Esta entidade representa um ativo financeiro que pode ser custodiado ou negociado pelo usuário.
 * Seu objetivo é encapsular regras de negócio fundamentais sobre a identidade e identificação do ativo,
 * garantindo que os dados sejam validados por objetos de valor específicos.
 */
public sealed class Asset
{
    public Asset(
        IdValueObject id,
        AssetCodeValueObject code,
        AssetNameValueObject name)
    {
        Id = id;
        Code = code;
        Name = name;
    }

    public IdValueObject Id { get; set; }
    public AssetCodeValueObject Code { get; set; }
    public AssetNameValueObject Name { get; set; }

    public bool IsValid => Id.IsValid && Code.IsValid && Name.IsValid;

    public IList<Operation>? Operations { get; set; } = null;
    public IList<PositionSnapshot>? PositionSnapshots { get; set; } = null;
    public IList<Quotation>? Quotations { get; set; } = null;
}

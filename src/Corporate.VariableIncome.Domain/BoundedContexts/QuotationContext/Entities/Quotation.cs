using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.Entities;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Domain.BoundedContexts.QuotationContext.Entities;

public sealed class Quotation
{
    public IdValueObject Id { get; set; }
    public IdValueObject AssetId { get; set; }
    public PriceValueObject UnitaryPrice { get; set; }
    public DateTimeValueObject DateTime { get; set; }

    public Quotation(IdValueObject id, PriceValueObject unitaryPrice, DateTimeValueObject dateTime)
    {
        Id = id;
        UnitaryPrice = unitaryPrice;
        DateTime = dateTime;
    }

    public Asset? Relationship1AssetNQuotations { get; set; }
}

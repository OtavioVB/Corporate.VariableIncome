using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.Services.Internal.QuotationContext.Inputs;

public readonly struct AppendAssetQuotationServiceInput
{
    public IdValueObject QuotationId { get; }
    public IdValueObject AssetId { get; }
    public PriceValueObject UnitaryPrice { get; }
    public DateTimeValueObject DateTime { get; }

    private AppendAssetQuotationServiceInput(IdValueObject quotationId, IdValueObject assetId, PriceValueObject unitaryPrice, DateTimeValueObject dateTime)
    {
        QuotationId = quotationId;
        AssetId = assetId;
        UnitaryPrice = unitaryPrice;
        DateTime = dateTime;
    }

    public static AppendAssetQuotationServiceInput Build(IdValueObject quotationId, IdValueObject assetId, PriceValueObject unitaryPrice, DateTimeValueObject dateTime)
        => new(quotationId, assetId, unitaryPrice, dateTime);

    public bool IsValid => QuotationId.IsValid && AssetId.IsValid && UnitaryPrice.IsValid && DateTime.IsValid;
}

using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.UseCases.PositionContext.RequestUpdatePosition.Inputs;

public readonly struct RequestUpdatePositionUseCaseInput
{
    public IdValueObject QuotationId { get; }
    public IdValueObject AssetId { get; }
    public PriceValueObject UnitaryPrice { get; }
    public DateTimeValueObject DateTime { get; }

    private RequestUpdatePositionUseCaseInput(IdValueObject quotationId, IdValueObject assetId, PriceValueObject unitaryPrice, DateTimeValueObject dateTime)
    {
        QuotationId = quotationId;
        AssetId = assetId;
        UnitaryPrice = unitaryPrice;
        DateTime = dateTime;
    }

    public static RequestUpdatePositionUseCaseInput Factory(
        IdValueObject quotationId, IdValueObject assetId, PriceValueObject unitaryPrice, DateTimeValueObject dateTime)
        => new(quotationId, assetId, unitaryPrice, dateTime);
}

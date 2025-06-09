using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.UseCases.QuotationContext.UpdateQuotationRealtime.Inputs;

public readonly struct UpdateQuotationRealtimeUseCaseInput
{
    public IdValueObject IdempotencyKey { get; }
    public IdValueObject AssetId { get; }
    public PriceValueObject UnitaryPrice { get; }
    public DateTimeValueObject DateTime { get; }

    private UpdateQuotationRealtimeUseCaseInput(IdValueObject idempotencyKey, IdValueObject assetId, PriceValueObject unitaryPrice, DateTimeValueObject dateTime)
    {
        IdempotencyKey = idempotencyKey;
        AssetId = assetId;
        UnitaryPrice = unitaryPrice;
        DateTime = dateTime;
    }

    public static UpdateQuotationRealtimeUseCaseInput Build(IdValueObject idempotencyKey, IdValueObject assetId, PriceValueObject unitaryPrice, DateTimeValueObject dateTime)
        => new(idempotencyKey, assetId, unitaryPrice, dateTime);
}

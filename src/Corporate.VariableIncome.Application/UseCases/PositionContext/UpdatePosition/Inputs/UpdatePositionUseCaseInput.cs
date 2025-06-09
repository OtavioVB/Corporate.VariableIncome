using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.UseCases.PositionContext.UpdatePosition.Inputs;

public readonly struct UpdatePositionUseCaseInput
{
    public UpdatePositionUseCaseInputPosition Position { get; }
    public UpdatePositionUseCaseInputQuotation Quotation { get; }

    private UpdatePositionUseCaseInput(UpdatePositionUseCaseInputPosition position, UpdatePositionUseCaseInputQuotation quotation)
    {
        Position = position;
        Quotation = quotation;
    }

    public static UpdatePositionUseCaseInput Factory(UpdatePositionUseCaseInputPosition position, UpdatePositionUseCaseInputQuotation quotation)
        => new(position, quotation);
}

public readonly struct UpdatePositionUseCaseInputPosition
{
    public IdValueObject AssetId { get; }
    public IdValueObject UserId { get; }
    public QuantityValueObject Quantity { get; }
    public PriceValueObject AveragePrice { get; }
    public DateTimeValueObject DateTime { get; }
    public ProftAndLossValueObject ProftAndLoss { get; }

    private UpdatePositionUseCaseInputPosition(IdValueObject assetId, IdValueObject userId, QuantityValueObject quantity, PriceValueObject averagePrice, DateTimeValueObject dateTime, ProftAndLossValueObject proftAndLoss)
    {
        AssetId = assetId;
        UserId = userId;
        Quantity = quantity;
        AveragePrice = averagePrice;
        DateTime = dateTime;
        ProftAndLoss = proftAndLoss;
    }

    public static UpdatePositionUseCaseInputPosition Factory(IdValueObject assetId, IdValueObject userId, QuantityValueObject quantity, PriceValueObject averagePrice, DateTimeValueObject dateTime, ProftAndLossValueObject proftAndLoss)
        => new(assetId, userId, quantity, averagePrice, dateTime, proftAndLoss);

    public bool IsValid => AssetId.IsValid & UserId.IsValid & Quantity.IsValid & AveragePrice.IsValid & DateTime.IsValid & ProftAndLoss.IsValid;
}

public readonly struct UpdatePositionUseCaseInputQuotation
{
    public IdValueObject AssetId { get; }
    public IdValueObject QuotationId { get; }
    public PriceValueObject UnitaryPrice { get; }
    public DateTimeValueObject DateTime { get; }

    private UpdatePositionUseCaseInputQuotation(IdValueObject assetId, IdValueObject quotationId, PriceValueObject unitaryPrice, DateTimeValueObject dateTime)
    {
        AssetId = assetId;
        QuotationId = quotationId;
        UnitaryPrice = unitaryPrice;
        DateTime = dateTime;
    }

    public static UpdatePositionUseCaseInputQuotation Factory(IdValueObject assetId, IdValueObject quotationId, PriceValueObject unitaryPrice, DateTimeValueObject dateTime)
        => new(assetId, quotationId, unitaryPrice, dateTime);

    public bool IsValid => AssetId.IsValid & QuotationId.IsValid & UnitaryPrice.IsValid & DateTime.IsValid;
}

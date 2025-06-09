using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.Services.Internal.PositionContext.Inputs;

public readonly struct CreatePositionSnapshotBasedOnQuotationUpdateServiceInput
{
    public CreatePositionSnapshotBasedOnQuotationUpdateServiceInputPosition Position { get; }
    public CreatePositionSnapshotBasedOnQuotationUpdateServiceInputQuotation Quotation { get; }

    private CreatePositionSnapshotBasedOnQuotationUpdateServiceInput(CreatePositionSnapshotBasedOnQuotationUpdateServiceInputPosition position, CreatePositionSnapshotBasedOnQuotationUpdateServiceInputQuotation quotation)
    {
        Position = position;
        Quotation = quotation;
    }

    public static CreatePositionSnapshotBasedOnQuotationUpdateServiceInput Factory(CreatePositionSnapshotBasedOnQuotationUpdateServiceInputPosition position, CreatePositionSnapshotBasedOnQuotationUpdateServiceInputQuotation quotation)
        => new(position, quotation);
}

public readonly struct CreatePositionSnapshotBasedOnQuotationUpdateServiceInputPosition
{
    public IdValueObject AssetId { get; }
    public IdValueObject UserId { get; }
    public QuantityValueObject Quantity { get; }
    public PriceValueObject AveragePrice { get; }
    public DateTimeValueObject DateTime { get; }
    public ProftAndLossValueObject ProftAndLoss { get; }

    private CreatePositionSnapshotBasedOnQuotationUpdateServiceInputPosition(IdValueObject assetId, IdValueObject userId, QuantityValueObject quantity, PriceValueObject averagePrice, DateTimeValueObject dateTime, ProftAndLossValueObject proftAndLoss)
    {
        AssetId = assetId;
        UserId = userId;
        Quantity = quantity;
        AveragePrice = averagePrice;
        DateTime = dateTime;
        ProftAndLoss = proftAndLoss;
    }

    public static CreatePositionSnapshotBasedOnQuotationUpdateServiceInputPosition Factory(IdValueObject assetId, IdValueObject userId, QuantityValueObject quantity, PriceValueObject averagePrice, DateTimeValueObject dateTime, ProftAndLossValueObject proftAndLoss)
        => new(assetId, userId, quantity, averagePrice, dateTime, proftAndLoss);

    public bool IsValid => AssetId.IsValid & UserId.IsValid & Quantity.IsValid & AveragePrice.IsValid & DateTime.IsValid & ProftAndLoss.IsValid;
}

public readonly struct CreatePositionSnapshotBasedOnQuotationUpdateServiceInputQuotation
{
    public IdValueObject AssetId { get; }
    public IdValueObject QuotationId { get; }
    public PriceValueObject UnitaryPrice { get; }
    public DateTimeValueObject DateTime { get; }

    private CreatePositionSnapshotBasedOnQuotationUpdateServiceInputQuotation(IdValueObject assetId, IdValueObject quotationId, PriceValueObject unitaryPrice, DateTimeValueObject dateTime)
    {
        AssetId = assetId;
        QuotationId = quotationId;
        UnitaryPrice = unitaryPrice;
        DateTime = dateTime;
    }

    public static CreatePositionSnapshotBasedOnQuotationUpdateServiceInputQuotation Factory(IdValueObject assetId, IdValueObject quotationId, PriceValueObject unitaryPrice, DateTimeValueObject dateTime)
        => new(assetId, quotationId, unitaryPrice, dateTime);

    public bool IsValid => AssetId.IsValid & QuotationId.IsValid & UnitaryPrice.IsValid & DateTime.IsValid;
}


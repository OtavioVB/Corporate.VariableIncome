using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.Services.Internal.OperationsContext.Inputs;

public readonly struct CreateSellOperationServiceInput
{
    public IdValueObject AssetId { get; }
    public IdValueObject UserId { get; }
    public PriceValueObject UnitaryPrice { get; }
    public QuantityValueObject Quantity { get; }
    public BrokerageFeeValueObject BrokerageFee { get; }

    private CreateSellOperationServiceInput(IdValueObject assetId, IdValueObject userId, PriceValueObject unitaryPrice, QuantityValueObject quantity, BrokerageFeeValueObject brokerageFee)
    {
        AssetId = assetId;
        UserId = userId;
        UnitaryPrice = unitaryPrice;
        Quantity = quantity;
        BrokerageFee = brokerageFee;
    }

    public static CreateSellOperationServiceInput Factory(IdValueObject assetId, IdValueObject userId, PriceValueObject unitaryPrice, QuantityValueObject quantity, BrokerageFeeValueObject brokerageFee)
        => new(assetId, userId, unitaryPrice, quantity, brokerageFee);

    public bool IsValid => AssetId.IsValid & UserId.IsValid & UnitaryPrice.IsValid & Quantity.IsValid & BrokerageFee.IsValid;
}

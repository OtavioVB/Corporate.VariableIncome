using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies.Models;

public readonly struct OperationStrategyModel
{
    public IdValueObject AssetId { get; }
    public IdValueObject UserId { get; }
    public QuantityValueObject Quantity { get; }
    public PriceValueObject UnitaryPrice { get; }
    public BrokerageFeeValueObject BrokerageFee { get; }

    private OperationStrategyModel(
        IdValueObject assetId, IdValueObject userId, QuantityValueObject quantity, PriceValueObject unitaryPrice,
        BrokerageFeeValueObject brokerageFee)
    {
        AssetId = assetId;
        UserId = userId;
        Quantity = quantity;
        UnitaryPrice = unitaryPrice;
        BrokerageFee = brokerageFee;
    }

    public static OperationStrategyModel Factory(IdValueObject assetId, IdValueObject userId, QuantityValueObject quantity, 
        PriceValueObject unitaryPrice, BrokerageFeeValueObject brokerageFee)
        => new(assetId, userId, quantity, unitaryPrice, brokerageFee);
}

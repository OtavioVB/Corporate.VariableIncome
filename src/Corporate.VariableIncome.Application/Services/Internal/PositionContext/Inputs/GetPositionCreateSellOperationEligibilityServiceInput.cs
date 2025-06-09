using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.Services.Internal.PositionContext.Inputs;

public readonly struct GetPositionCreateSellOperationEligibilityServiceInput
{
    public IdValueObject AssetId { get; }
    public IdValueObject UserId { get; }
    public QuantityValueObject Quantity { get; }

    private GetPositionCreateSellOperationEligibilityServiceInput(IdValueObject assetId, IdValueObject userId, QuantityValueObject quantity)
    {
        AssetId = assetId;
        UserId = userId;
        Quantity = quantity;
    }

    public static GetPositionCreateSellOperationEligibilityServiceInput Factory(IdValueObject assetId, IdValueObject userId, QuantityValueObject quantity)
        => new(assetId, userId, quantity);

    public bool IsValid => AssetId.IsValid && UserId.IsValid && Quantity.IsValid;
}

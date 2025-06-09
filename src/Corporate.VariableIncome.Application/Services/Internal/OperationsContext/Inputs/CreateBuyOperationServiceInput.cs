using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.Services.Internal.OperationsContext.Inputs;

public readonly struct CreateBuyOperationServiceInput
{
    public IdValueObject AssetId { get; }
    public IdValueObject UserId { get; }
    public PriceValueObject UnitaryPrice { get; }
    public QuantityValueObject Quantity { get; }
    public BrokerageFeeValueObject BrokerageFee { get; }

    private CreateBuyOperationServiceInput(IdValueObject assetId, IdValueObject userId, PriceValueObject unitaryPrice, QuantityValueObject quantity, BrokerageFeeValueObject brokerageFee)
    {
        AssetId = assetId;
        UserId = userId;
        UnitaryPrice = unitaryPrice;
        Quantity = quantity;
        BrokerageFee = brokerageFee;
    }

    public static CreateBuyOperationServiceInput Build(IdValueObject assetId, IdValueObject userId, PriceValueObject unitaryPrice, QuantityValueObject quantity, BrokerageFeeValueObject brokerageFee)
        => new(assetId, userId, unitaryPrice, quantity, brokerageFee);

    public bool IsValid => AssetId.IsValid && UserId.IsValid && UnitaryPrice.IsValid && Quantity.IsValid && BrokerageFee.IsValid;

    public string ErrorMessage => "É necessário que os campos para compra de ativo financeiro sejam válidos.";
}

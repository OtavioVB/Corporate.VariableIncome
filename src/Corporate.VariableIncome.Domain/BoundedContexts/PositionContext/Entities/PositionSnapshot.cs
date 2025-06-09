using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.UserContext.Entities;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Domain.BoundedContexts.PositionContext.Entities;

public sealed class PositionSnapshot
{
    public PositionSnapshot(
        IdValueObject id, 
        IdValueObject assetId, 
        IdValueObject userId, 
        QuantityValueObject quantity, 
        PriceValueObject averagePrice, 
        DateTimeValueObject dateTime, 
        ProftAndLossValueObject proftAndLoss)
    {
        Id = id;
        AssetId = assetId;
        UserId = userId;
        Quantity = quantity;
        AveragePrice = averagePrice;
        DateTime = dateTime;
        ProftAndLoss = proftAndLoss;
    }

    public IdValueObject Id { get; set; }
    public IdValueObject AssetId { get; set; }
    public IdValueObject UserId { get; set; }
    public QuantityValueObject Quantity { get; set; }
    public PriceValueObject AveragePrice { get; set; }
    public DateTimeValueObject DateTime { get; set; }
    public ProftAndLossValueObject ProftAndLoss { get; set; }
    
    public Asset? Relationship1AssetNPositionSnapshots { get; set; }
    public User? Relationship1UserNPositionSnapshots { get; set; }

    public bool IsValid => Id.IsValid && AssetId.IsValid && UserId.IsValid && Quantity.IsValid && AveragePrice.IsValid && DateTime.IsValid && ProftAndLoss.IsValid;
    public bool CanSellRequestedQuantity(int quantity)
        => quantity <= Quantity.GetValue();
}

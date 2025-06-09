using Corporate.VariableIncome.Domain.BoundedContexts.PositionContext.Entities;

namespace Corporate.VariableIncome.WebApi.Controllers.PositionsContext.Responses;

public sealed record GetUserActualPositionResponse
{
    public string Id { get; }
    public string AssetId { get; }
    public string UserId { get; }
    public int Quantity { get; }
    public decimal AveragePrice { get; }
    public DateTime DateTime { get; }
    public decimal ProftAndLoss { get; }

    private GetUserActualPositionResponse(string id, string assetId, string userId, int quantity, decimal averagePrice, DateTime dateTime, decimal proftAndLoss)
    {
        Id = id;
        AssetId = assetId;
        UserId = userId;
        Quantity = quantity;
        AveragePrice = averagePrice;
        DateTime = dateTime;
        ProftAndLoss = proftAndLoss;
    }

    public static GetUserActualPositionResponse Factory(PositionSnapshot positionSnapshot)
        => new(positionSnapshot.Id.GetValueAsString(), positionSnapshot.AssetId.GetValueAsString(), positionSnapshot.UserId.GetValueAsString(), positionSnapshot.Quantity, positionSnapshot.AveragePrice, positionSnapshot.DateTime, positionSnapshot.ProftAndLoss);
}

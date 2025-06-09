using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Enumerators;
using System.Text.Json.Serialization;

namespace Corporate.VariableIncome.WebApi.Controllers.OperationsContext.Requests;

public readonly struct CreateOperationRequest
{
    public CreateOperationRequest(Guid assetId, Guid userId, int quantity, EnumTypeOperation operation)
    {
        AssetId = assetId;
        UserId = userId;
        Quantity = quantity;
        Operation = operation;
    }

    public Guid AssetId { get; init; }
    public Guid UserId { get; init; }
    public int Quantity { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EnumTypeOperation Operation { get; init; }
}

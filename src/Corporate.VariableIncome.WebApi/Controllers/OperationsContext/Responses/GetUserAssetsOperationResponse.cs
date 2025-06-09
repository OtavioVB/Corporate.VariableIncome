using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;

namespace Corporate.VariableIncome.WebApi.Controllers.OperationsContext.Responses;

public sealed record GetUserAssetsOperationResponse
{
    public GetUserAssetsOperationResponseOperation[] Operations { get; }

    private GetUserAssetsOperationResponse(GetUserAssetsOperationResponseOperation[] operations)
    {
        Operations = operations;
    }

    public static GetUserAssetsOperationResponse Factory(Operation[] operations)
        => new(operations.Select(p => GetUserAssetsOperationResponseOperation.Factory(p)).ToArray());
}

public sealed record GetUserAssetsOperationResponseOperation
{
    public string OperationId { get; }
    public string Type { get; }
    public decimal UnitaryPrice { get; }
    public int Quantity { get; }
    public decimal BrokerageeFee { get; }
    public DateTime DateTime { get; }

    private GetUserAssetsOperationResponseOperation(string operationId, string type, decimal unitaryPrice, int quantity, decimal brokerageeFee, DateTime dateTime)
    {
        OperationId = operationId;
        Type = type;
        UnitaryPrice = unitaryPrice;
        Quantity = quantity;
        BrokerageeFee = brokerageeFee;
        DateTime = dateTime;
    }

    public static GetUserAssetsOperationResponseOperation Factory(Operation operation)
        => new(operation.Id.GetValueAsString(), operation.Type, operation.UnitaryPrice, operation.Quantity, operation.BrokerageFee, operation.DateTime);
}

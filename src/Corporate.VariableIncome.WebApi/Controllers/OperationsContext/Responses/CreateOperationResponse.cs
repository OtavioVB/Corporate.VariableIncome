using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.PositionContext.Entities;

namespace Corporate.VariableIncome.WebApi.Controllers.OperationsContext.Responses;

public sealed record CreateOperationResponse
{
    public CreateOperationResponsePosition Position { get; }
    public CreateOperationResponseOperation Operation { get; }

    private CreateOperationResponse(CreateOperationResponsePosition position, CreateOperationResponseOperation operation)
    {
        Position = position;
        Operation = operation;
    }

    public static CreateOperationResponse Build(PositionSnapshot position, Operation operation)
        => new(CreateOperationResponsePosition.Build(position), CreateOperationResponseOperation.Build(operation));
}

public sealed record CreateOperationResponsePosition
{
    public string PositionId { get; set; }
    public int Quantity { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal ProftAndLoss { get; set; }
    public DateTime DateTime { get; }

    private CreateOperationResponsePosition(string positionId, int quantity, decimal averagePrice, decimal proftAndLoss, DateTime dateTime)
    {
        PositionId = positionId;
        Quantity = quantity;
        AveragePrice = averagePrice;
        ProftAndLoss = proftAndLoss;
        DateTime = dateTime;
    }

    public static CreateOperationResponsePosition Build(PositionSnapshot position)
        => new(
            positionId: position.Id.GetValueAsString(),
            quantity: position.Quantity,
            averagePrice: position.AveragePrice,
            proftAndLoss: position.ProftAndLoss,
            dateTime: position.DateTime);
}

public sealed record CreateOperationResponseOperation
{
    public string OperationId { get; }
    public string Type { get; }
    public decimal UnitaryPrice { get; }
    public int Quantity { get; }
    public decimal BrokerageeFee { get; }
    public DateTime DateTime { get; }

    private CreateOperationResponseOperation(string operationId, string type, decimal unitaryPrice, int quantity, decimal brokerageeFee, DateTime dateTime)
    {
        OperationId = operationId;
        Type = type;
        UnitaryPrice = unitaryPrice;
        Quantity = quantity;
        BrokerageeFee = brokerageeFee;
        DateTime = dateTime;
    }

    public static CreateOperationResponseOperation Build(Operation operation)
        => new(
            operationId: operation.Id.GetValueAsString(),
            type: operation.Id.GetValueAsString(),
            unitaryPrice: operation.UnitaryPrice,
            quantity: operation.Quantity,
            brokerageeFee: operation.BrokerageFee,
            dateTime: operation.DateTime);
}

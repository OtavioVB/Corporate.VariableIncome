using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.PositionContext.Entities;

namespace Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Outputs;

public readonly struct CreateOperationUseCaseOutput
{
    public Operation Operation { get; }
    public PositionSnapshot Position { get; }

    private CreateOperationUseCaseOutput(Operation operation, PositionSnapshot position)
    {
        Operation = operation;
        Position = position;
    }

    public static CreateOperationUseCaseOutput Factory(Operation operation, PositionSnapshot position)
        => new(operation, position);
}

using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;

namespace Corporate.VariableIncome.Application.UseCases.OperationContext.GetUserOperations.Outputs;

public readonly struct GetAssetUserOperationsUseCaseOutput
{
    public Operation[] Operations { get; }

    private GetAssetUserOperationsUseCaseOutput(Operation[] operations)
    {
        Operations = operations;
    }

    public static GetAssetUserOperationsUseCaseOutput Factory(Operation[] operations)
        => new(operations);
}

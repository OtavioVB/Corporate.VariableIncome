using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Outputs;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies.Models;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;
using Corporate.VariableIncome.Domain.Helpers;

namespace Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies.Interfaces;

public interface IOperationStrategy
{
    public Task<Result<CreateOperationUseCaseOutput>> CreateOperationAsync(
        OperationStrategyModel model,
        CancellationToken cancellationToken);
}

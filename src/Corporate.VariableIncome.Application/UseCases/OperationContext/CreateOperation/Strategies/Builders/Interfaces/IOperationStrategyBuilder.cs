using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies.Interfaces;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.ValueObjects;

namespace Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies.Builders.Interfaces;

public interface IOperationStrategyBuilder
{
    public IOperationStrategy BuildStrategyBehavior(TypeOperationValueObject typeOperation);
}

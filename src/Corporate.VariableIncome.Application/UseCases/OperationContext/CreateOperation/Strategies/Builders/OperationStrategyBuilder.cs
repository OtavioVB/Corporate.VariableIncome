using Corporate.VariableIncome.Application.Services.Internal.OperationsContext.Interfaces;
using Corporate.VariableIncome.Application.Services.Internal.PositionContext.Interfaces;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies.Builders.Interfaces;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies.Interfaces;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Enumerators;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies.Builders;

public sealed class OperationStrategyBuilder : IOperationStrategyBuilder
{
    private readonly IServiceProvider _serviceProvider;

    public OperationStrategyBuilder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IOperationStrategy BuildStrategyBehavior(TypeOperationValueObject typeOperation)
    {
        switch (typeOperation.GetValue())
        {
            case EnumTypeOperation.SELL:
                return new SellOperationStrategy(
                    operationService: _serviceProvider.GetRequiredService<IOperationService>(),
                    positionService: _serviceProvider.GetRequiredService<IPositionService>());
            case EnumTypeOperation.BUY:
                return new BuyOperationStrategy(
                    operationService: _serviceProvider.GetRequiredService<IOperationService>(),
                    positionService: _serviceProvider.GetRequiredService<IPositionService>());
            default:
                throw new NotImplementedException();
        }
    }   
}

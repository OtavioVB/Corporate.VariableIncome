using Corporate.VariableIncome.Application.Services.Internal.OperationsContext.Inputs;
using Corporate.VariableIncome.Application.Services.Internal.OperationsContext.Interfaces;
using Corporate.VariableIncome.Application.Services.Internal.PositionContext.Interfaces;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Outputs;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies.Interfaces;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies.Models;
using Corporate.VariableIncome.Domain.Helpers;

namespace Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies;

public sealed class BuyOperationStrategy : IOperationStrategy
{
    private readonly IOperationService _operationService;
    private readonly IPositionService _positionService;

    public BuyOperationStrategy(
        IOperationService operationService,
        IPositionService positionService)
    {
        _operationService = operationService;
        _positionService = positionService;
    }

    public async Task<Result<CreateOperationUseCaseOutput>> CreateOperationAsync(
        OperationStrategyModel model, 
        CancellationToken cancellationToken)
    {
        var createOperationServiceResult = await _operationService.CreateBuyOperationServiceAsync(
            input: CreateBuyOperationServiceInput.Build(
                assetId: model.AssetId,
                userId: model.UserId,
                unitaryPrice: model.UnitaryPrice,
                quantity: model.Quantity,
                brokerageFee: model.BrokerageFee),
            cancellationToken: cancellationToken);

        if (createOperationServiceResult.IsFailed)
            return Result<CreateOperationUseCaseOutput>.Error(
                message: createOperationServiceResult.GetRequiredMessage());

        var createPositionSnapshotServiceResult = await _positionService.CreatePositionSnapshotBasedOnBuyOperationAsync(
            buyOperation: createOperationServiceResult.GetValue(),
            cancellationToken: cancellationToken);

        if (createPositionSnapshotServiceResult.IsFailed)
            return Result<CreateOperationUseCaseOutput>.Error(
                message: createPositionSnapshotServiceResult.GetRequiredMessage());

        var output = CreateOperationUseCaseOutput.Factory(
            operation: createOperationServiceResult.GetValue(),
            position: createPositionSnapshotServiceResult.GetValue());

        return Result<CreateOperationUseCaseOutput>.Success(
            value: output);
    }
}

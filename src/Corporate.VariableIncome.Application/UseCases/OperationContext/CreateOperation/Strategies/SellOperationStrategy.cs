using Corporate.VariableIncome.Application.Services.Internal.OperationsContext.Inputs;
using Corporate.VariableIncome.Application.Services.Internal.OperationsContext.Interfaces;
using Corporate.VariableIncome.Application.Services.Internal.PositionContext.Inputs;
using Corporate.VariableIncome.Application.Services.Internal.PositionContext.Interfaces;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Outputs;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies.Interfaces;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies.Models;
using Corporate.VariableIncome.Domain.Helpers;

namespace Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies;

public sealed class SellOperationStrategy : IOperationStrategy
{
    private readonly IPositionService _positionService;
    private readonly IOperationService _operationService;

    public SellOperationStrategy(
         IPositionService positionService,
         IOperationService operationService)
    {
        _positionService = positionService;
        _operationService = operationService;
    }

    public async Task<Result<CreateOperationUseCaseOutput>> CreateOperationAsync(OperationStrategyModel model, CancellationToken cancellationToken)
    {
        var createSellOperationEligibilityServiceResult = await _positionService.GetPositionCreateSellOperationEligibilityServiceAsync(
            input: GetPositionCreateSellOperationEligibilityServiceInput.Factory(
                assetId: model.AssetId,
                userId: model.UserId,
                quantity: model.Quantity),
            cancellationToken: cancellationToken);

        if (createSellOperationEligibilityServiceResult.IsFailed)
            return Result<CreateOperationUseCaseOutput>.Error(
                message: createSellOperationEligibilityServiceResult.GetRequiredMessage());

        var createSellOperationServiceResult = await _operationService.CreateSellOperationServiceAsync(
            input: CreateSellOperationServiceInput.Factory(
                assetId: model.AssetId,
                userId: model.UserId,
                unitaryPrice: model.UnitaryPrice,
                quantity: model.Quantity,
                brokerageFee: model.BrokerageFee),
            cancellationToken: cancellationToken);

        if (createSellOperationServiceResult.IsFailed)
            return Result<CreateOperationUseCaseOutput>.Error(
                message: createSellOperationServiceResult.GetRequiredMessage());

        var createNewPositionSnapshotServiceResult = await _positionService.CreatePositionSnapshotBasedOnSellOperationAsync(
            sellOperation: createSellOperationServiceResult.GetValue(),
            latestSnapshot: createSellOperationEligibilityServiceResult.GetValue(),
            cancellationToken: cancellationToken);

        if (createNewPositionSnapshotServiceResult.IsFailed)
            return Result<CreateOperationUseCaseOutput>.Error(
                message: createNewPositionSnapshotServiceResult.GetRequiredMessage());

        var output = CreateOperationUseCaseOutput.Factory(
            operation: createSellOperationServiceResult.GetValue(),
            position: createNewPositionSnapshotServiceResult.GetValue());

        return Result<CreateOperationUseCaseOutput>.Success(
            value: output);
    }
}

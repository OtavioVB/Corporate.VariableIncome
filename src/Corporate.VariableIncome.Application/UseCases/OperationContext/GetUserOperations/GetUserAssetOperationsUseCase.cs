using Corporate.VariableIncome.Application.Services.Internal.OperationsContext.Inputs;
using Corporate.VariableIncome.Application.Services.Internal.OperationsContext.Interfaces;
using Corporate.VariableIncome.Application.UseCases.Interfaces;
using Corporate.VariableIncome.Application.UseCases.OperationContext.GetUserOperations.Inputs;
using Corporate.VariableIncome.Application.UseCases.OperationContext.GetUserOperations.Outputs;
using Corporate.VariableIncome.Domain.Helpers;
using Microsoft.Extensions.Logging;

namespace Corporate.VariableIncome.Application.UseCases.OperationContext.GetUserOperations;

public sealed class GetUserAssetOperationsUseCase : IUseCase<GetAssetUserOperationsUseCaseInput, Result<GetAssetUserOperationsUseCaseOutput>>
{
    private readonly ILogger<GetUserAssetOperationsUseCase> _logger;
    private readonly IOperationService _operationService;

    public GetUserAssetOperationsUseCase(ILogger<GetUserAssetOperationsUseCase> logger, IOperationService operationService)
    {
        _logger = logger;
        _operationService = operationService;
    }

    public async Task<Result<GetAssetUserOperationsUseCaseOutput>> ExecuteUseCaseAsync(GetAssetUserOperationsUseCaseInput input, CancellationToken cancellationToken)
    {
        try
        {
            var getOperationServiceResult = await _operationService.GetUserAssetOperationsServiceAsync(
                input: GetUserAssetOperationsServiceInput.Factory(
                    userId: input.UserId,
                    assetId: input.AssetId,
                    page: input.Page,
                    offset: input.Offset,
                    startDate: input.StartDate,
                    endDate: input.EndDate),
                cancellationToken: cancellationToken);

            if (getOperationServiceResult.IsFailed)
                return Result<GetAssetUserOperationsUseCaseOutput>.Error(getOperationServiceResult.GetRequiredMessage());

            return Result<GetAssetUserOperationsUseCaseOutput>.Success(
                GetAssetUserOperationsUseCaseOutput.Factory(
                    operations: getOperationServiceResult.GetValue()));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                exception: ex,
                message: "[{Type}][{Method}] Is not possible to execute use case because unhandled exception has been throwed.",
                nameof(GetUserAssetOperationsUseCase),
                nameof(ExecuteUseCaseAsync));

            return Result<GetAssetUserOperationsUseCaseOutput>.Error(
                message: "Um erro inesperado ocorreu durante o processamento de consulta de operações do usuário.");
        }
    }
}

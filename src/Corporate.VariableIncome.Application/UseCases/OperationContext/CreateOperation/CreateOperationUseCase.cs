using Corporate.VariableIncome.Application.Services.Internal.AssetContext.Interfaces;
using Corporate.VariableIncome.Application.Services.Internal.QuotationContext.Interfaces;
using Corporate.VariableIncome.Application.Services.Internal.UserContext.Interfaces;
using Corporate.VariableIncome.Application.UseCases.Interfaces;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Inputs;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Outputs;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies.Builders.Interfaces;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies.Models;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation;

public sealed class CreateOperationUseCase : IUseCase<CreateOperationUseCaseInput, Result<CreateOperationUseCaseOutput>>
{
    private readonly ILogger<CreateOperationUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQuotationService _quotationService;
    private readonly IOperationStrategyBuilder _operationStrategyBuilder;
    private readonly IUserService _userService;
    private readonly IAssetService _assetService;

    public CreateOperationUseCase(
        ILogger<CreateOperationUseCase> logger,
        IUnitOfWork unitOfWork, 
        IQuotationService quotationService,
        IUserService userService,
        IAssetService assetService,
        IOperationStrategyBuilder operationStrategyBuilder)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _quotationService = quotationService;
        _userService = userService;
        _assetService = assetService;
        _operationStrategyBuilder = operationStrategyBuilder;
    }

    public Task<Result<CreateOperationUseCaseOutput>> ExecuteUseCaseAsync(
        CreateOperationUseCaseInput input, 
        CancellationToken cancellationToken)
    {
        try
        {
            return _unitOfWork.ExecuteUnitOfWorkAsync(
                input: input,
                handler: async (input, cancellationToken) =>
                {
                    var userEligibilityServiceResult = await _userService.GetCreateOperationUserEligibilityAsync(
                        userId: input.UserId,
                        cancellationToken: cancellationToken);

                    if (userEligibilityServiceResult.IsFailed)
                        return (Commit: false, Output: Result<CreateOperationUseCaseOutput>.Error(
                            message: userEligibilityServiceResult.GetRequiredMessage()));

                    var assetEligibilityServiceResult = await _assetService.GetAssetCreateOperationElegibilityAsync(
                        assetId: input.AssetId,
                        cancellationToken: cancellationToken);

                    if (assetEligibilityServiceResult.IsFailed)
                        return (Commit: false, Output: Result<CreateOperationUseCaseOutput>.Error(
                            message: assetEligibilityServiceResult.GetRequiredMessage()));

                    var quotationServiceResult = await _quotationService.QueryLatestAssetQuotationAsync(
                        assetId: input.AssetId,
                        cancellationToken: cancellationToken);

                    if (quotationServiceResult.IsFailed)
                        return (Commit: false, Output: Result<CreateOperationUseCaseOutput>.Error(
                            message: quotationServiceResult.GetRequiredMessage()));

                    var operationStrategy = _operationStrategyBuilder.BuildStrategyBehavior(
                        typeOperation: input.TypeOperation);

                    var createOperationResult = await operationStrategy.CreateOperationAsync(
                        model: OperationStrategyModel.Factory(
                            assetId: assetEligibilityServiceResult.GetValue().Id,
                            userId: userEligibilityServiceResult.GetValue().Id,
                            quantity: input.Quantity,
                            unitaryPrice: quotationServiceResult.GetValue().UnitaryPrice,
                            brokerageFee: userEligibilityServiceResult.GetValue().BrokerageFee),
                        cancellationToken: cancellationToken);

                    if (createOperationResult.IsFailed)
                        return (Commit: false, Output: Result<CreateOperationUseCaseOutput>.Error(
                            message: createOperationResult.GetRequiredMessage()));

                    return (Commit: true, Output: Result<CreateOperationUseCaseOutput>.Success(
                        value: createOperationResult.GetValue()));
                },
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}][{Method}] Is not possible to execute create operation use case, because unhandled exception has been throwed.",
                nameof(CreateOperationUseCase),
                nameof(ExecuteUseCaseAsync));

            return Task.FromResult(Result<CreateOperationUseCaseOutput>.Error(
                message: "Não é possível realizar a criação da operação sobre o ativo financeiro."));
        }
    }
}

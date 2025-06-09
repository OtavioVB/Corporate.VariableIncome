using Corporate.VariableIncome.Application.Services.External.B3FinanceAsset.Interfaces;
using Corporate.VariableIncome.Application.Services.Internal.AssetContext.Interfaces;
using Corporate.VariableIncome.Application.Services.Internal.PositionContext.Inputs;
using Corporate.VariableIncome.Application.Services.Internal.PositionContext.Interfaces;
using Corporate.VariableIncome.Application.Services.Internal.QuotationContext.Inputs;
using Corporate.VariableIncome.Application.Services.Internal.QuotationContext.Interfaces;
using Corporate.VariableIncome.Application.UseCases.Interfaces;
using Corporate.VariableIncome.Application.UseCases.PositionContext.GetActualPosition.Inputs;
using Corporate.VariableIncome.Domain.BoundedContexts.PositionContext.Entities;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Domain.ValueObjects;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace Corporate.VariableIncome.Application.UseCases.PositionContext.GetUserActualPosition;

public sealed class GetUserActualPositionUseCase : IUseCase<GetUserActualPositionUseCaseInput, Result<PositionSnapshot>>
{
    private readonly ILogger<GetUserActualPositionUseCase> _logger;
    private readonly IB3FinanceAssetService _b3FinanceAssetService;
    private readonly IAssetService _assetService;
    private readonly IQuotationService _quotationService;
    private readonly IPositionService _positionService;
    private readonly IUnitOfWork _unitOfWork;

    public GetUserActualPositionUseCase(
        ILogger<GetUserActualPositionUseCase> logger, 
        IB3FinanceAssetService b3FinanceAssetService, 
        IAssetService assetService, 
        IQuotationService quotationService, 
        IPositionService positionService, 
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _b3FinanceAssetService = b3FinanceAssetService;
        _assetService = assetService;
        _quotationService = quotationService;
        _positionService = positionService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PositionSnapshot>> ExecuteUseCaseAsync(GetUserActualPositionUseCaseInput input, CancellationToken cancellationToken)
    {
        try
        {
            return await _unitOfWork.ExecuteUnitOfWorkAsync(
                input: input,
                handler: async (input, cancellationToken) =>
                {
                    var queryAssetServiceResult = await _assetService.GetAssetByCodeAsync(
                        code: input.Code,
                        cancellationToken: cancellationToken);

                    if (queryAssetServiceResult.IsFailed)
                        return (Commit: false, Result<PositionSnapshot>.Error(queryAssetServiceResult.GetRequiredMessage()));

                    var queryAssetQuotationActualServiceResult = await _b3FinanceAssetService.GetFinanceAssetQuotationServiceAsync(
                        code: input.Code,
                        cancellationToken: cancellationToken);

                    if (queryAssetQuotationActualServiceResult.IsFailed)
                        return (Commit: false, Result<PositionSnapshot>.Error(queryAssetQuotationActualServiceResult.GetRequiredMessage()));

                    var registerNewQuotationServiceResult = await _quotationService.AppendAssetQuotationAsync(
                        input: AppendAssetQuotationServiceInput.Build(
                            quotationId: IdValueObject.Build(),
                            assetId: queryAssetServiceResult.GetValue().Id,
                            unitaryPrice: queryAssetQuotationActualServiceResult.GetValue().price,
                            dateTime: queryAssetQuotationActualServiceResult.GetValue().tradetime),
                        cancellationToken: cancellationToken);

                    if (registerNewQuotationServiceResult.IsFailed)
                        return (Commit: false, Result<PositionSnapshot>.Error(registerNewQuotationServiceResult.GetRequiredMessage()));

                    var queryLatestUserPositionServiceResult = await _positionService.GetUserLatestPositionServiceAsync(
                        userId: input.UserId,
                        assetId: queryAssetServiceResult.GetValue().Id,
                        cancellationToken: cancellationToken);

                    if (queryLatestUserPositionServiceResult.IsFailed)
                        return (Commit: false, Result<PositionSnapshot>.Error(queryLatestUserPositionServiceResult.GetRequiredMessage()));

                    var position = queryLatestUserPositionServiceResult.GetValue();

                    var quotation = registerNewQuotationServiceResult.GetValue();

                    var registerNewPositionServiceResult = await _positionService.CreatePositionSnapshotBasedOnQuotationUpdateAsync(
                        input: CreatePositionSnapshotBasedOnQuotationUpdateServiceInput.Factory(
                            position: CreatePositionSnapshotBasedOnQuotationUpdateServiceInputPosition.Factory(
                                assetId: queryAssetServiceResult.GetValue().Id,
                                userId: position.UserId,
                                quantity: position.Quantity,
                                averagePrice: position.AveragePrice,
                                dateTime: position.DateTime,
                                proftAndLoss: position.ProftAndLoss),
                            quotation: CreatePositionSnapshotBasedOnQuotationUpdateServiceInputQuotation.Factory(
                                assetId: queryAssetServiceResult.GetValue().Id,
                                quotationId: quotation.Id,
                                unitaryPrice: quotation.UnitaryPrice,
                                dateTime: quotation.DateTime)),
                        cancellationToken: cancellationToken);

                    if (queryLatestUserPositionServiceResult.IsFailed)
                        return (Commit: false, Result<PositionSnapshot>.Error(queryLatestUserPositionServiceResult.GetRequiredMessage()));

                    return (Commit: true, Result<PositionSnapshot>.Success(registerNewPositionServiceResult.GetValue()));
                },
                cancellationToken: cancellationToken);
        }
        catch (Exception ex) 
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}][{Method}] An unhandled exception has been throweid get user asset positions use case executing.",
                nameof(GetUserActualPositionUseCase),
                nameof(ExecuteUseCaseAsync));

            return Result<PositionSnapshot>.Error(
                message: "[{Type}][{Method}] An unhandled exception has been throweid get user asset positions use case executing.");
        }
    }
}

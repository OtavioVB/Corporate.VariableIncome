using Corporate.VariableIncome.Application.Services.Internal.PositionContext.Inputs;
using Corporate.VariableIncome.Application.Services.Internal.PositionContext.Interfaces;
using Corporate.VariableIncome.Application.UseCases.Interfaces;
using Corporate.VariableIncome.Application.UseCases.PositionContext.UpdatePosition.Inputs;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace Corporate.VariableIncome.Application.UseCases.PositionContext.UpdatePosition;

public sealed class UpdatePositionUseCase : IUseCase<UpdatePositionUseCaseInput, Result>
{
    private readonly ILogger<UpdatePositionUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPositionService _positionService;

    public UpdatePositionUseCase(ILogger<UpdatePositionUseCase> logger, IUnitOfWork unitOfWork, IPositionService positionService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _positionService = positionService;
    }

    public async Task<Result> ExecuteUseCaseAsync(UpdatePositionUseCaseInput input, CancellationToken cancellationToken)
    {
        try
        {
            return await _unitOfWork.ExecuteUnitOfWorkAsync(
                input: input,
                handler: async (input, cancellationToken) =>
                {
                    var createPositionServiceResult = await _positionService.CreatePositionSnapshotBasedOnQuotationUpdateAsync(
                        input: CreatePositionSnapshotBasedOnQuotationUpdateServiceInput.Factory(
                            position: CreatePositionSnapshotBasedOnQuotationUpdateServiceInputPosition.Factory(
                                assetId: input.Position.AssetId,
                                userId: input.Position.UserId,
                                quantity: input.Position.Quantity,
                                averagePrice: input.Position.AveragePrice,
                                dateTime: input.Position.DateTime,
                                proftAndLoss: input.Position.ProftAndLoss),
                            quotation: CreatePositionSnapshotBasedOnQuotationUpdateServiceInputQuotation.Factory(
                                assetId: input.Quotation.AssetId,
                                quotationId: input.Quotation.QuotationId,
                                unitaryPrice: input.Quotation.UnitaryPrice,
                                dateTime: input.Quotation.DateTime)),
                        cancellationToken: cancellationToken);

                    if (createPositionServiceResult.IsFailed)
                        return (Commit: false, Result.Error(createPositionServiceResult.GetRequiredMessage()));

                    return (Commit: true, Output: Result.Success());
                },
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}][{Method}] Is not possible to update user positons because unhandled exception has been throwed.",
                nameof(UpdatePositionUseCase),
                nameof(ExecuteUseCaseAsync));

            return Result.Error("Um erro inesperado ocorreu durante o processamento da posição do usuário para o ativo financeiro.");
        }
    }
}

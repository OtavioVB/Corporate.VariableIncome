using Corporate.VariableIncome.Application.Services.Internal.PositionContext.Inputs;
using Corporate.VariableIncome.Application.Services.Internal.PositionContext.Interfaces;
using Corporate.VariableIncome.Application.Services.Internal.PositionContext.Outputs;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.PositionContext.Entities;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Domain.ValueObjects;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Base.Interfaces;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Extensions;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace Corporate.VariableIncome.Application.Services.Internal.PositionContext;

public sealed class PositionService : IPositionService
{
    private readonly ILogger<PositionService> _logger;
    private readonly IExtensionPositionSnapshotRepository _extensionPositionSnapshotRepository;
    private readonly IBaseRepository<PositionSnapshot> _positionSnapshotBaseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PositionService(
        ILogger<PositionService> logger,
        IExtensionPositionSnapshotRepository extensionPositionSnapshotRepository,
        IBaseRepository<PositionSnapshot> positionSnapshotBaseRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _extensionPositionSnapshotRepository = extensionPositionSnapshotRepository;
        _positionSnapshotBaseRepository = positionSnapshotBaseRepository;
        _unitOfWork = unitOfWork;
    }

    private PriceValueObject CalculatePriceAverage(
        QuantityValueObject actualQuantity,
        PriceValueObject actualPriceAverage,
        QuantityValueObject buyQuantity,
        PriceValueObject buyPrice)
    {
        var actual = actualPriceAverage * actualQuantity;

        var bought = buyPrice * buyQuantity;

        var totalQuantity = buyQuantity + actualQuantity;

        var priceAverage = (actual + bought) / totalQuantity;

        return priceAverage;
    }

    private ProftAndLossValueObject CalculateProfAndLoss(
        PriceValueObject actualPrice,
        PriceValueObject averagePrice)
    {
        var proftAndLoss = (actualPrice - averagePrice) / averagePrice;

        return proftAndLoss;
    }

    public async Task<Result<PositionSnapshot>> CreatePositionSnapshotBasedOnQuotationUpdateAsync(
        CreatePositionSnapshotBasedOnQuotationUpdateServiceInput input,
        CancellationToken cancellationToken)
    {
        try
        {
            var positionSnapshot = new PositionSnapshot(
                id: IdValueObject.Build(),
                assetId: input.Position.AssetId,
                userId: input.Position.UserId,
                quantity: input.Position.Quantity,
                averagePrice: input.Position.AveragePrice,
                dateTime: input.Quotation.DateTime,
                proftAndLoss: CalculateProfAndLoss(
                    actualPrice: input.Quotation.UnitaryPrice,
                    averagePrice: input.Position.AveragePrice));

            await _positionSnapshotBaseRepository.AddAsync(positionSnapshot, cancellationToken);
            await _unitOfWork.ApplyTransactionAsync(cancellationToken);

            return Result<PositionSnapshot>.Success(
                value: positionSnapshot);
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}][{Method}] Create position based on quotation realtime update is not possible because unhandled exception has been throwed. Info = {@Info}",
                nameof(PositionService),
                nameof(CreatePositionSnapshotBasedOnQuotationUpdateAsync),
                new
                {
                    AssetId = input.Position.AssetId.GetValueAsString(),
                    UserId = input.Position.UserId.GetValueAsString(),
                    QuotationId = input.Quotation.QuotationId.GetValueAsString(),
                    QuotationDateTime = input.Quotation.DateTime.GetValue(),
                    PositionDateTime = input.Position.DateTime.GetValue(),
                });

            return Result<PositionSnapshot>.Error(
                "Não foi po´ssível atualizar a posição do usuário baseada na cotação por um erro inesperado.");
        }
    }

    public async Task<Result<PositionSnapshot>> CreatePositionSnapshotBasedOnBuyOperationAsync(
        Operation buyOperation,
        CancellationToken cancellationToken)
    {
        try
        {
            var latestSnapshot = await _extensionPositionSnapshotRepository.GetLatestPositionSnapshotAsNoTrackingByAssetIdAndUserIdAsync(
                assetId: buyOperation.AssetId,
                userId: buyOperation.UserId,
                cancellationToken: cancellationToken);

            if (latestSnapshot == null)
            {
                var basePositionSnapshot = new PositionSnapshot(
                    id: IdValueObject.Build(),
                    assetId: buyOperation.AssetId,
                    userId: buyOperation.UserId,
                    quantity: buyOperation.Quantity,
                    averagePrice: buyOperation.UnitaryPrice,
                    dateTime: DateTimeValueObject.Build(),
                    proftAndLoss: 0);

                await _positionSnapshotBaseRepository.AddAsync(basePositionSnapshot, cancellationToken);
                await _unitOfWork.ApplyTransactionAsync(cancellationToken);

                return Result<PositionSnapshot>.Success(
                    value: basePositionSnapshot);
            }

            var priceAverage = CalculatePriceAverage(
                actualQuantity: latestSnapshot.Quantity,
                actualPriceAverage: latestSnapshot.AveragePrice,
                buyQuantity: buyOperation.Quantity,
                buyPrice: buyOperation.UnitaryPrice);

            var newPositionSnapshot = new PositionSnapshot(
                id: IdValueObject.Build(),
                assetId: latestSnapshot.AssetId,
                userId: latestSnapshot.UserId,
                quantity: latestSnapshot.Quantity + buyOperation.Quantity,
                averagePrice: priceAverage,
                dateTime: DateTimeValueObject.Build(),
                proftAndLoss: latestSnapshot.ProftAndLoss);

            await _positionSnapshotBaseRepository.AddAsync(newPositionSnapshot, cancellationToken);
            await _unitOfWork.ApplyTransactionAsync(cancellationToken);

            return Result<PositionSnapshot>.Success(
                value: newPositionSnapshot);
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}][{Method}] Is not possible to create position snapshot because an unhandled exception has been throwed.",
                nameof(PositionService),
                nameof(CreatePositionSnapshotBasedOnBuyOperationAsync));

            return Result<PositionSnapshot>.Error(
                message: "Não foi possível cadastrar a nova posição do usuário.");
        }
    }

    public async Task<Result<PositionSnapshot>> CreatePositionSnapshotBasedOnSellOperationAsync(
        Operation sellOperation,
        PositionSnapshot latestSnapshot,
        CancellationToken cancellationToken)
    {
        try
        {
            var newPositionSnapshot = new PositionSnapshot(
                id: IdValueObject.Build(),
                assetId: latestSnapshot.AssetId,
                userId: latestSnapshot.UserId,
                quantity: latestSnapshot.Quantity - sellOperation.Quantity,
                averagePrice: latestSnapshot.AveragePrice,
                dateTime: DateTimeValueObject.Build(),
                proftAndLoss: CalculateProfAndLoss(
                    actualPrice: sellOperation.UnitaryPrice,
                    averagePrice: latestSnapshot.AveragePrice));

            await _positionSnapshotBaseRepository.AddAsync(newPositionSnapshot, cancellationToken);
            await _unitOfWork.ApplyTransactionAsync(cancellationToken);

            return Result<PositionSnapshot>.Success(
                value: newPositionSnapshot);
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}][{Method}] Is not possible to create position snapshot because an unhandled exception has been throwed.",
                nameof(PositionService),
                nameof(CreatePositionSnapshotBasedOnSellOperationAsync));

            return Result<PositionSnapshot>.Error(
                message: "Não foi possível cadastrar a nova posição do usuário.");
        }
    }

    public async Task<Result<PositionSnapshot>> GetPositionCreateSellOperationEligibilityServiceAsync(GetPositionCreateSellOperationEligibilityServiceInput input, CancellationToken cancellationToken)
    {
        try
        {
            if (!input.IsValid)
                return Result<PositionSnapshot>.Error(
                    message: "Para consultar a elegibilidade da posição para a operação de venda de ativo financeiro é necessário enviar dados válidos.");

            var position = await _extensionPositionSnapshotRepository.GetLatestPositionSnapshotAsNoTrackingByAssetIdAndUserIdAsync(
                assetId: input.AssetId,
                userId: input.UserId,
                cancellationToken: cancellationToken);

            if (position == null)
                return Result<PositionSnapshot>.Error(
                    message: "Não há nenhuma posição para usuário que possa permitir realizar a operação de venda do ativo financeiro.");

            if (!position.CanSellRequestedQuantity(input.Quantity))
                return Result<PositionSnapshot>.Error(
                    message: "Não é possível vender uma quantidade de ativos financeiros maior que sua posição atual.");

            return Result<PositionSnapshot>.Success(
                value: position);
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}][{Method}] Is not possible to query postion eligibility to make sell operation because unhandled exception has been throwed by application. Input = {@Input}",
                nameof(PositionService),
                nameof(GetPositionCreateSellOperationEligibilityServiceAsync),
                new
                {
                    input.IsValid,
                    UserId = input.UserId.GetValue(),
                    AssetId = input.AssetId.GetValue(),
                    Quantity = input.Quantity.GetValue()
                });

            return Result<PositionSnapshot>.Error(
                message: "Não foi possível consultar a elegibilidade da sua posição para venda do ativo financeiro.");
        }
    }

    public async Task<Result<GetUserLatestPositionPaginationByAssetServiceOutput>> GetUserLatestPositionPaginationByAssetServiceAsync(
        GetUserLatestPositionPaginationByAssetServiceInput input,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!input.IsValid)
                return Result<GetUserLatestPositionPaginationByAssetServiceOutput>.Error(
                    message: "É necessário enviar dados válidos para consultar as posições paginadas de usuários pelo ativo financeiro.");

            var positionsSnapshots = await _extensionPositionSnapshotRepository.GetLatestPositionSnapshotAsNoTrackingByAssetIdAsync(
                assetId: input.AssetId,
                page: input.Page,
                offset: input.Offset,
                cancellationToken: cancellationToken);

            var output = GetUserLatestPositionPaginationByAssetServiceOutput.Factory(
                hasMorePages: input.Offset.GetValue() == positionsSnapshots.Length,
                positions: positionsSnapshots);

            return Result<GetUserLatestPositionPaginationByAssetServiceOutput>.Success(
                value: output);
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}][{Method}] Is not possible to query user paginated positions by asset because unhandled exception has been throwed by application. Input = {@Input}",
                nameof(PositionService),
                nameof(GetUserLatestPositionPaginationByAssetServiceAsync),
                new
                {
                    input.IsValid,
                    AssetId = input.AssetId.GetValue(),
                    Page = input.Page.GetValue(),
                    Offset = input.Offset.GetValue()
                });

            return Result<GetUserLatestPositionPaginationByAssetServiceOutput>.Error(
                    message: "Um erro inesperado ocorreu ao consultar as posições paginadas de usuários pelo ativo financeiro.");
        }
    }

    public async Task<Result<PositionSnapshot>> GetUserLatestPositionServiceAsync(
        IdValueObject userId, 
        IdValueObject assetId, 
        CancellationToken cancellationToken)
    {
        try
        {
            if (!(userId.IsValid && assetId.IsValid))
                return Result<PositionSnapshot>.Error(
                    "É necessário enviar dados inválidos do ativo financeiro e do usuário para consultar a posição.");

            var position = await _extensionPositionSnapshotRepository.GetLatestPositionSnapshotAsNoTrackingByAssetIdAndUserIdAsync(
                assetId: assetId,
                userId: userId,
                cancellationToken: cancellationToken);

            if (position is null)
                return Result<PositionSnapshot>.Error(
                    "Não foi possivel encontrar nenhuma posição para esse cliente e usuário.");

            return Result<PositionSnapshot>.Success(position);
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}][{Method}] An unhandled exception has been throwed get user latest position. Input = {@Input}",
                nameof(PositionService),
                nameof(GetUserLatestPositionServiceAsync),
                new
                {
                    IsValid = userId.IsValid && assetId.IsValid,
                    UserId = userId.GetValueAsString(),
                    AssetId = assetId.GetValueAsString(),
                });

            return Result<PositionSnapshot>.Error("Não foi possível consultar a última posição do usuário por um erro inesperado.");
        }
    }
}

using Corporate.VariableIncome.Application.Services.Internal.PositionContext.Inputs;
using Corporate.VariableIncome.Application.Services.Internal.PositionContext.Outputs;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.PositionContext.Entities;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.Services.Internal.PositionContext.Interfaces;

public interface IPositionService
{
    public Task<Result<PositionSnapshot>> GetPositionCreateSellOperationEligibilityServiceAsync(
        GetPositionCreateSellOperationEligibilityServiceInput input,
        CancellationToken cancellationToken);

    public Task<Result<PositionSnapshot>> CreatePositionSnapshotBasedOnBuyOperationAsync(
        Operation buyOperation,
        CancellationToken cancellationToken);
    public Task<Result<PositionSnapshot>> CreatePositionSnapshotBasedOnSellOperationAsync(
        Operation sellOperation,
        PositionSnapshot latestSnapshot,
        CancellationToken cancellationToken);

    public Task<Result<PositionSnapshot>> CreatePositionSnapshotBasedOnQuotationUpdateAsync(
        CreatePositionSnapshotBasedOnQuotationUpdateServiceInput input,
        CancellationToken cancellationToken);

    public Task<Result<GetUserLatestPositionPaginationByAssetServiceOutput>> GetUserLatestPositionPaginationByAssetServiceAsync(
        GetUserLatestPositionPaginationByAssetServiceInput input,
        CancellationToken cancellationToken);

    public Task<Result<PositionSnapshot>> GetUserLatestPositionServiceAsync(
        IdValueObject userId,
        IdValueObject assetId,
        CancellationToken cancellationToken);
}

using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.ValueObjects;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.Services.Internal.AssetContext.Interfaces;

public interface IAssetService
{
    public Task<Result<Asset>> GetAssetCreateOperationElegibilityAsync(
        IdValueObject assetId,
        CancellationToken cancellationToken);

    public Task<Result> GetAssetUpdateQuotationRealtimeElegibilityAsync(
        IdValueObject assetId,
        CancellationToken cancellationToken);

    public Task<Result<Asset>> GetAssetByCodeAsync(
        AssetCodeValueObject code,
        CancellationToken cancellationToken);
}

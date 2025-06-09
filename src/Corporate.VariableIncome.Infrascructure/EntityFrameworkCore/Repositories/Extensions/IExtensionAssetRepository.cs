using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.ValueObjects;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Extensions;

public interface IExtensionAssetRepository
{
    public Task<bool> VerifyAssetExistsByCodeAsync(AssetCodeValueObject code, CancellationToken cancellationToken);
    public Task<bool> VerifyAssetExistsByIdAsync(IdValueObject assetId, CancellationToken cancellationToken);
    public Task<Asset?> GetAssetAsNoTrackingByIdAsync(IdValueObject assetId, CancellationToken cancellationToken);
    public Task<Asset?> GetAssetByCodeAsNoTrackingAsync(AssetCodeValueObject code, CancellationToken cancellationToken);
}

using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.ValueObjects;
using Corporate.VariableIncome.Domain.ValueObjects;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Base;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories;

public sealed class AssetRepository : BaseRepository<Asset>, IExtensionAssetRepository
{
    public AssetRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public Task<Asset?> GetAssetAsNoTrackingByIdAsync(IdValueObject assetId, CancellationToken cancellationToken)
        => _dataContext.Set<Asset>().AsNoTracking().Where(p => p.Id == assetId.GetValue()).FirstOrDefaultAsync(cancellationToken);

    public Task<Asset?> GetAssetByCodeAsNoTrackingAsync(AssetCodeValueObject code, CancellationToken cancellationToken)
    {
        var assetLocal = _dataContext.Set<Asset>().Local.Where(p => p.Code == code).FirstOrDefault();

        if (assetLocal is not null)
            return Task.FromResult((Asset?)assetLocal);

        return _dataContext.Set<Asset>().AsNoTracking().Where(p => p.Code == code).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> VerifyAssetExistsByCodeAsync(AssetCodeValueObject code, CancellationToken cancellationToken)
    {
        var userLocal = _dataContext.Set<Asset>().Local.Where(p => p.Code == code).Any();

        if (userLocal)
            return Task.FromResult(true);

        return _dataContext.Set<Asset>().AsNoTracking().Where(p => p.Code == code).AnyAsync(cancellationToken);
    }

    public Task<bool> VerifyAssetExistsByIdAsync(IdValueObject assetId, CancellationToken cancellationToken)
        => _dataContext.Set<Asset>().AsNoTracking().Where(p => p.Id == assetId.GetValue()).AnyAsync(cancellationToken);
}

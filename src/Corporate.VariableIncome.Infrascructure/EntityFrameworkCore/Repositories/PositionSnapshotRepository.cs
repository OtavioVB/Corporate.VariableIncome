using Corporate.VariableIncome.Domain.BoundedContexts.PositionContext.Entities;
using Corporate.VariableIncome.Domain.ValueObjects;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Base;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories;

public sealed class PositionSnapshotRepository : BaseRepository<PositionSnapshot>, IExtensionPositionSnapshotRepository
{
    public PositionSnapshotRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public Task<PositionSnapshot[]> GetLatestPositionSnapshotAsNoTrackingByAssetIdAsync(
        IdValueObject assetId,
        PageValueObject page,
        OffsetValueObject offset,
        CancellationToken cancellationToken)
        => _dataContext.Set<PositionSnapshot>().AsNoTracking()
            .Where(p => p.AssetId == assetId.GetValue())
            .GroupBy(x => new { x.UserId, x.AssetId })
            .Select(g => g.OrderByDescending(x => x.DateTime).First())
            .Skip(page.GetSkip(offset))
            .Take(offset)
            .ToArrayAsync(cancellationToken);

    public Task<PositionSnapshot?> GetLatestPositionSnapshotAsNoTrackingByAssetIdAndUserIdAsync(IdValueObject assetId, IdValueObject userId, CancellationToken cancellationToken)
        => _dataContext.Set<PositionSnapshot>().AsNoTracking().Where(p => p.AssetId == assetId.GetValue() && p.UserId == userId.GetValue()).OrderByDescending(p => p.DateTime).FirstOrDefaultAsync(cancellationToken);
}
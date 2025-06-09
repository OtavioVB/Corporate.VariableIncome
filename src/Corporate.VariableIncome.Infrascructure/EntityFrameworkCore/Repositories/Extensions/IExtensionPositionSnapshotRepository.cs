using Corporate.VariableIncome.Domain.BoundedContexts.PositionContext.Entities;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Extensions;

public interface IExtensionPositionSnapshotRepository
{
    public Task<PositionSnapshot?> GetLatestPositionSnapshotAsNoTrackingByAssetIdAndUserIdAsync(
        IdValueObject assetId,
        IdValueObject userId,
        CancellationToken cancellationToken);

    public Task<PositionSnapshot[]> GetLatestPositionSnapshotAsNoTrackingByAssetIdAsync(
        IdValueObject assetId,
        PageValueObject page,
        OffsetValueObject offset,
        CancellationToken cancellationToken);
}

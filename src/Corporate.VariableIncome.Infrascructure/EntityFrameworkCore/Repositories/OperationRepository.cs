using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;
using Corporate.VariableIncome.Domain.ValueObjects;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Base;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories;

public sealed class OperationRepository : BaseRepository<Operation>, IExtensionOperationRepository
{
    public OperationRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public Task<Operation[]> GetUserAssetOperationsAsync(IdValueObject assetId, IdValueObject userId, DateTimeValueObject startDate, DateTimeValueObject endDate, PageValueObject page, OffsetValueObject offset, CancellationToken cancellationToken)
        => _dataContext.Set<Operation>().AsNoTracking()
        .Where(p => p.AssetId == assetId.GetValue() 
            && p.UserId == userId.GetValue() 
            && p.DateTime > startDate.GetValue() 
            && p.DateTime < endDate.GetValue())
        .OrderByDescending(p => p.DateTime)
        .Skip(page.GetSkip(offset))
        .Take(offset.GetValue()).ToArrayAsync(cancellationToken);
}

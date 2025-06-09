using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Extensions;

public interface IExtensionOperationRepository
{
    public Task<Operation[]> GetUserAssetOperationsAsync(
        IdValueObject assetId,
        IdValueObject userId,
        DateTimeValueObject startDate,
        DateTimeValueObject endDate,
        PageValueObject page,
        OffsetValueObject offset,
        CancellationToken cancellationToken);
}

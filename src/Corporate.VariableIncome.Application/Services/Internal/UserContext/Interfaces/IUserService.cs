using Corporate.VariableIncome.Domain.BoundedContexts.UserContext.Entities;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.Services.Internal.UserContext.Interfaces;

public interface IUserService
{
    public Task<Result<User>> GetCreateOperationUserEligibilityAsync(
        IdValueObject userId,
        CancellationToken cancellationToken);
}

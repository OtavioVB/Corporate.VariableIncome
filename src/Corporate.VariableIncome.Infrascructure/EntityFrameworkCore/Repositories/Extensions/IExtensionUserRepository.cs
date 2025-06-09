using Corporate.VariableIncome.Domain.BoundedContexts.UserContext.Entities;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Extensions;

public interface IExtensionUserRepository
{
    public Task<bool> VerifyUserExistsByEmailAsync(EmailValueObject email, CancellationToken cancellationToken);
    public Task<User?> GetUserAsNoTrackingByIdAsync(IdValueObject userId, CancellationToken cancellationToken);
}

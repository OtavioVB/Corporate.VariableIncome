using Corporate.VariableIncome.Domain.BoundedContexts.UserContext.Entities;
using Corporate.VariableIncome.Domain.ValueObjects;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Base;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories;

public sealed class UserRepository : BaseRepository<User>, IExtensionUserRepository
{
    public UserRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public Task<User?> GetUserAsNoTrackingByIdAsync(IdValueObject userId, CancellationToken cancellationToken)
        => _dataContext.Set<User>().AsNoTracking().Where(p => p.Id == userId.GetValue()).FirstOrDefaultAsync(cancellationToken);

    public Task<bool> VerifyUserExistsByEmailAsync(EmailValueObject email, CancellationToken cancellationToken)
    {
        var userLocal = _dataContext.Set<User>().Local.Where(p => p.Email == email).Any();

        if (userLocal)
            return Task.FromResult(true);

        return _dataContext.Set<User>().AsNoTracking().Where(p => p.Email == email).AnyAsync(cancellationToken);
    }
}

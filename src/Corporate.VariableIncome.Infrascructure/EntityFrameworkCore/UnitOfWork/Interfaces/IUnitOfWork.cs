using System.Data;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;

public interface IUnitOfWork
{
    public Task ApplyTransactionAsync(CancellationToken cancellationToken);

    public Task<TOutput> ExecuteUnitOfWorkAsync<TInput, TOutput>(
        TInput input,
        Func<TInput, CancellationToken, Task<(bool Commit, TOutput Output)>> handler,
        CancellationToken cancellationToken,
        IsolationLevel isolationLevel = IsolationLevel.RepeatableRead);
}

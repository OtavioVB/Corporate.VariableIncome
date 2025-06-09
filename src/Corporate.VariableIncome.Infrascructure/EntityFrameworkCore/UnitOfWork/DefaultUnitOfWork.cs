using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;
using System.Data;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.UnitOfWork;

public sealed class DefaultUnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;

    public DefaultUnitOfWork(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public Task ApplyTransactionAsync(CancellationToken cancellationToken)
        => _dataContext.SaveChangesAsync(cancellationToken);

    public async Task<TOutput> ExecuteUnitOfWorkAsync<TInput, TOutput>(
        TInput input, 
        Func<TInput, CancellationToken, Task<(bool Commit, TOutput Output)>> handler, 
        CancellationToken cancellationToken, 
        IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
    {
        var transaction = await _dataContext.Database.BeginTransactionAsync(cancellationToken);
        var handlerResponse = await handler(input, cancellationToken);

        if (!handlerResponse.Commit)
        {
            await transaction.RollbackAsync(cancellationToken);
            _ = transaction.DisposeAsync();
            return handlerResponse.Output;
        }
        else
        {
            await transaction.CommitAsync(cancellationToken);
            _ = transaction.DisposeAsync();
            return handlerResponse.Output;
        }
    }
}
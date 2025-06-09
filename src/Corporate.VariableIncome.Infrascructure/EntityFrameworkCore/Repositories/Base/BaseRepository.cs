using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Base.Interfaces;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Base;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class
{
    protected readonly DataContext _dataContext;

    protected BaseRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        => _dataContext.Set<TEntity>().AddAsync(entity, cancellationToken).AsTask();

    public Task AddRangeAsync(TEntity[] entities, CancellationToken cancellationToken)
        => _dataContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        => Task.Run(() => _dataContext.Set<TEntity>().Remove(entity), cancellationToken);

    public Task DeleteRangeAsync(TEntity[] entities, CancellationToken cancellationToken)
        => Task.Run(() => _dataContext.Set<TEntity>().RemoveRange(entities), cancellationToken);

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        => Task.Run(() => _dataContext.Set<TEntity>().Update(entity), cancellationToken);

    public Task UpdateRangeAsync(TEntity[] entities, CancellationToken cancellationToken)
        => Task.Run(() => _dataContext.Set<TEntity>().UpdateRange(entities), cancellationToken);
}

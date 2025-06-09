namespace Corporate.VariableIncome.Application.UseCases.Interfaces;

public interface IUseCase<TInput, TOutput>
{
    public Task<TOutput> ExecuteUseCaseAsync(TInput input, CancellationToken cancellationToken);
}

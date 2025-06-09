using Corporate.VariableIncome.Application.Services.Internal.OperationsContext.Inputs;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;
using Corporate.VariableIncome.Domain.Helpers;

namespace Corporate.VariableIncome.Application.Services.Internal.OperationsContext.Interfaces;

public interface IOperationService
{
    public Task<Result<Operation>> CreateBuyOperationServiceAsync(
        CreateBuyOperationServiceInput input,
        CancellationToken cancellationToken);

    public Task<Result<Operation>> CreateSellOperationServiceAsync(
        CreateSellOperationServiceInput input,
        CancellationToken cancellationToken);

    public Task<Result<Operation[]>> GetUserAssetOperationsServiceAsync(
        GetUserAssetOperationsServiceInput input,
        CancellationToken cancellationToken);
}

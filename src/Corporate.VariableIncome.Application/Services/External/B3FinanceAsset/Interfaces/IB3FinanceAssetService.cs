using Corporate.VariableIncome.Application.Services.External.B3FinanceAsset.Outputs;
using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.ValueObjects;
using Corporate.VariableIncome.Domain.Helpers;

namespace Corporate.VariableIncome.Application.Services.External.B3FinanceAsset.Interfaces;

public interface IB3FinanceAssetService
{
    public Task<Result<GetFinanceAssetQuotationServiceOutput>> GetFinanceAssetQuotationServiceAsync(
        AssetCodeValueObject code,
        CancellationToken cancellationToken);
}

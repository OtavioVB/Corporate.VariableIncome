using Corporate.VariableIncome.Application.Services.Internal.QuotationContext.Inputs;
using Corporate.VariableIncome.Domain.BoundedContexts.QuotationContext.Entities;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.Services.Internal.QuotationContext.Interfaces;

public interface IQuotationService
{
    public Task<Result<Quotation>> QueryLatestAssetQuotationAsync(
        IdValueObject assetId,
        CancellationToken cancellationToken);

    public Task<Result<Quotation>> AppendAssetQuotationAsync(
        AppendAssetQuotationServiceInput input,
        CancellationToken cancellationToken);
}

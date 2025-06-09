using Corporate.VariableIncome.Domain.BoundedContexts.QuotationContext.Entities;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Extensions;

public interface IExtensionQuotationRepository
{
    public Task<Quotation?> QueryLatestAssetQuotationAsNoTrackingAsync(
        IdValueObject assetId,
        CancellationToken cancellationToken);
    public Task<Quotation?> QueryAssetQuotationByIdAsNoTrackingAsync(
        IdValueObject quotationId,
        CancellationToken cancellationToken);
}

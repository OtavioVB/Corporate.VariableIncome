using Corporate.VariableIncome.Domain.BoundedContexts.QuotationContext.Entities;
using Corporate.VariableIncome.Domain.ValueObjects;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Base;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories;

public sealed class QuotationRepository : BaseRepository<Quotation>, IExtensionQuotationRepository
{
    public QuotationRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public Task<Quotation?> QueryAssetQuotationByIdAsNoTrackingAsync(IdValueObject quotationId, CancellationToken cancellationToken)
        => _dataContext.Set<Quotation>().AsNoTracking()
            .Where(p => p.Id == quotationId.GetValue()).FirstOrDefaultAsync(cancellationToken);

    public Task<Quotation?> QueryLatestAssetQuotationAsNoTrackingAsync(IdValueObject assetId, CancellationToken cancellationToken)
        => _dataContext.Set<Quotation>().AsNoTracking()
            .Where(p => p.AssetId == assetId.GetValue())
            .OrderByDescending(p => p.DateTime)
            .FirstOrDefaultAsync(cancellationToken);
}

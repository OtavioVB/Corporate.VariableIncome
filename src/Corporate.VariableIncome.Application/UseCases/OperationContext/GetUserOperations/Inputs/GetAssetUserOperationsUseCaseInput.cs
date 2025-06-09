using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.UseCases.OperationContext.GetUserOperations.Inputs;

public readonly struct GetAssetUserOperationsUseCaseInput
{
    public IdValueObject UserId { get; }
    public PageValueObject Page { get; }
    public OffsetValueObject Offset { get; }
    public DateTimeValueObject StartDate { get; }
    public DateTimeValueObject EndDate { get; }
    public IdValueObject AssetId { get; }

    private GetAssetUserOperationsUseCaseInput(IdValueObject userId, PageValueObject page, OffsetValueObject offset,
        DateTimeValueObject startDate, DateTimeValueObject endDate, IdValueObject assetId)
    {
        UserId = userId;
        Page = page;
        Offset = offset;
        StartDate = startDate;
        EndDate = endDate;
        AssetId = assetId;
    }

    public static GetAssetUserOperationsUseCaseInput Factory(IdValueObject userId, PageValueObject page, OffsetValueObject offset,
        DateTimeValueObject startDate, DateTimeValueObject endDate, IdValueObject assetId)
        => new(userId, page, offset, startDate, endDate, assetId);
}

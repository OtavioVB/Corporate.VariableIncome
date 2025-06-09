using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.Services.Internal.OperationsContext.Inputs;

public readonly struct GetUserAssetOperationsServiceInput
{
    public IdValueObject UserId { get; }
    public IdValueObject AssetId { get; }
    public PageValueObject Page { get; }
    public OffsetValueObject Offset { get; }
    public DateTimeValueObject StartDate { get; }
    public DateTimeValueObject EndDate { get; }

    private GetUserAssetOperationsServiceInput(IdValueObject userId, IdValueObject assetId, PageValueObject page, OffsetValueObject offset, DateTimeValueObject startDate, DateTimeValueObject endDate)
    {
        UserId = userId;
        AssetId = assetId;
        Page = page;
        Offset = offset;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static GetUserAssetOperationsServiceInput Factory(IdValueObject userId, IdValueObject assetId, PageValueObject page, OffsetValueObject offset, DateTimeValueObject startDate, DateTimeValueObject endDate)
        => new(userId, assetId, page, offset, startDate, endDate);

    public bool IsValid => UserId.IsValid && AssetId.IsValid && Page.IsValid && Offset.IsValid && StartDate.IsValid && EndDate.IsValid;

    public bool IsValidStartDateLessThanEndDate => IsValid ? StartDate.GetValue() < EndDate.GetValue() : false;
}

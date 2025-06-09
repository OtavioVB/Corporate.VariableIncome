using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.Services.Internal.PositionContext.Inputs;

public readonly struct GetUserLatestPositionPaginationByAssetServiceInput
{
    public PageValueObject Page { get; }
    public OffsetValueObject Offset { get; }
    public IdValueObject AssetId { get; }

    private GetUserLatestPositionPaginationByAssetServiceInput(PageValueObject page, OffsetValueObject offset, IdValueObject assetId)
    {
        Page = page;
        Offset = offset;
        AssetId = assetId;
    }

    public static GetUserLatestPositionPaginationByAssetServiceInput Factory(PageValueObject page, OffsetValueObject offset, IdValueObject assetId)
        => new(page, offset, assetId);

    public bool IsValid => Page.IsValid && Offset.IsValid && AssetId.IsValid;
}

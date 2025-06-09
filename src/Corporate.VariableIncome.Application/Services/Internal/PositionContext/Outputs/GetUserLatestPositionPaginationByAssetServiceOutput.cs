using Corporate.VariableIncome.Domain.BoundedContexts.PositionContext.Entities;

namespace Corporate.VariableIncome.Application.Services.Internal.PositionContext.Outputs;

public readonly struct GetUserLatestPositionPaginationByAssetServiceOutput
{
    public bool HasMorePages { get; }
    public PositionSnapshot[] Positions { get; }

    private GetUserLatestPositionPaginationByAssetServiceOutput(bool hasMorePages, PositionSnapshot[] positions)
    {
        HasMorePages = hasMorePages;
        Positions = positions;
    }

    public static GetUserLatestPositionPaginationByAssetServiceOutput Factory(bool hasMorePages, PositionSnapshot[] positions)
        => new(hasMorePages, positions);
}

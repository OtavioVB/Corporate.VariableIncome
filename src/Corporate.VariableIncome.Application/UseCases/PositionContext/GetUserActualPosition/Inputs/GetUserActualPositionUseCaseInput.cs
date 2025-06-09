using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.ValueObjects;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.UseCases.PositionContext.GetActualPosition.Inputs;

public readonly struct GetUserActualPositionUseCaseInput
{
    public AssetCodeValueObject Code { get; }
    public IdValueObject UserId { get; }

    private GetUserActualPositionUseCaseInput(AssetCodeValueObject code, IdValueObject userId)
    {
        Code = code;
        UserId = userId;
    }

    public static GetUserActualPositionUseCaseInput Factory(AssetCodeValueObject code, IdValueObject userId)
        => new(code, userId);
}

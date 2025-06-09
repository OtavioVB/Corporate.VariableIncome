using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.ValueObjects;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Inputs;

public readonly struct CreateOperationUseCaseInput
{
    public IdValueObject AssetId { get; }
    public IdValueObject UserId { get; }
    public QuantityValueObject Quantity { get; }
    public TypeOperationValueObject TypeOperation { get; }

    private CreateOperationUseCaseInput(IdValueObject assetId, IdValueObject userId, QuantityValueObject quantity, TypeOperationValueObject typeOperation)
    {
        AssetId = assetId;
        UserId = userId;
        Quantity = quantity;
        TypeOperation = typeOperation;
    }

    public static CreateOperationUseCaseInput Factory(IdValueObject assetId, IdValueObject userId, QuantityValueObject quantity, TypeOperationValueObject typeOperation)
        => new(assetId, userId, quantity, typeOperation);
}

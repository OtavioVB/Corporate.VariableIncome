using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Enumerators;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.ValueObjects;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.UnitTests.Domain.Entities;

public sealed class OperationAveragePriceCalculationTests
{
    [Fact(DisplayName = "Deve retornar erro se a lista de operações estiver vazia")]
    public void CalculateAveragePrice_WhenEmptyArray_ShouldReturnError()
    {
        // Arrange
        var operations = Array.Empty<Operation>();

        // Act
        var result = Operation.CalculateAveragePrice(operations);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal("Para calcular preço médio de operações de compra de ativo financeiro é necessário enviar mais de uma operação.", result.GetRequiredMessage());
    }

    [Fact(DisplayName = "Deve retornar erro se operação contiver atributo inválido")]
    public void CalculateAveragePrice_WhenOperationIsInvalid_ShouldReturnError()
    {
        // Arrange
        var op = new Operation(
            id: IdValueObject.Build(Guid.NewGuid()),
            unitaryPrice: PriceValueObject.Build(0), // inválido
            quantity: QuantityValueObject.Build(10),
            brokerageFee: BrokerageFeeValueObject.Build(0.01m),
            dateTime: DateTimeValueObject.Build(DateTime.UtcNow),
            type: TypeOperationValueObject.Build(EnumTypeOperation.BUY)
        )
        {
            AssetId = IdValueObject.Build(Guid.NewGuid()),
            UserId = IdValueObject.Build(Guid.NewGuid())
        };

        // Act
        var result = Operation.CalculateAveragePrice(new[] { op });

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal("É necessário que todos os atributos de uma operação de compra de ativo financeiro.", result.GetRequiredMessage());
    }

    [Fact(DisplayName = "Deve retornar erro se alguma operação não for do tipo BUY")]
    public void CalculateAveragePrice_WhenTypeIsNotBuy_ShouldReturnError()
    {
        // Arrange
        var op = BuildValidOperation(EnumTypeOperation.SELL);

        // Act
        var result = Operation.CalculateAveragePrice(new[] { op });

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal("As operações enviadas precisam ser operações de compra para cálcular preço médio.", result.GetRequiredMessage());
    }

    [Fact(DisplayName = "Deve retornar erro se a quantidade da operação for menor que 1")]
    public void CalculateAveragePrice_WhenQuantityIsLessThanOne_ShouldReturnError()
    {
        // Arrange
        var op = BuildValidOperation(EnumTypeOperation.BUY, quantity: 0);

        // Act
        var result = Operation.CalculateAveragePrice(new[] { op });

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal("As operações de compra de ativo financeiro precisa conter quantidade maior que 0.", result.GetRequiredMessage());
    }

    [Fact(DisplayName = "Deve calcular corretamente o preço médio de múltiplas operações de compra")]
    public void CalculateAveragePrice_WhenValidBuyOperations_ShouldReturnSuccessWithCorrectAverage()
    {
        // Arrange
        var op1 = BuildValidOperation(EnumTypeOperation.BUY, quantity: 1, price: 10.00m);
        var op2 = BuildValidOperation(EnumTypeOperation.BUY, quantity: 2, price: 20.00m);

        // Act
        var result = Operation.CalculateAveragePrice(new[] { op1, op2 });

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(16.67m, Math.Round(result.GetValue(), 2));
    }

    private Operation BuildValidOperation(EnumTypeOperation type, int quantity = 1, decimal price = 10.00m)
    {
        return new Operation(
            id: IdValueObject.Build(),
            unitaryPrice: PriceValueObject.Build(price),
            quantity: QuantityValueObject.Build(quantity),
            brokerageFee: BrokerageFeeValueObject.Build(0.02m),
            dateTime: DateTimeValueObject.Build(DateTime.UtcNow),
            type: TypeOperationValueObject.Build(type)
        )
        {
            AssetId = IdValueObject.Build(),
            UserId = IdValueObject.Build()
        };
    }
}

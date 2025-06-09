using Corporate.VariableIncome.Domain.Exceptions;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.UnitTests.Domain.ValueObjects;

public sealed class QuantityValueObjectValidationTests
{
    [Fact(DisplayName = "Build() com valor menor que 1 deve retornar objeto inválido")]
    public void Build_WithValueLessThanOne_ShouldReturnInvalid()
    {
        // Arrange
        var input = 0;

        // Act
        var result = QuantityValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com valor negativo deve retornar objeto inválido")]
    public void Build_WithNegativeValue_ShouldReturnInvalid()
    {
        // Arrange
        var input = -10;

        // Act
        var result = QuantityValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com valor válido deve retornar objeto válido")]
    public void Build_WithValidValue_ShouldReturnValidObject()
    {
        // Arrange
        var input = 5;

        // Act
        var result = QuantityValueObject.Build(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(input, result.GetValue());
    }

    [Fact(DisplayName = "GetValue() em objeto inválido deve lançar ValueObjectException")]
    public void GetValue_WhenInvalid_ShouldThrowException()
    {
        // Arrange
        var result = QuantityValueObject.Build(0);

        // Act & Assert
        Assert.Throws<ValueObjectException>(() => result.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita int -> QuantityValueObject deve funcionar")]
    public void ImplicitConversion_FromInt_ShouldReturnValidObject()
    {
        // Arrange
        int input = 12;

        // Act
        QuantityValueObject quantity = input;

        // Assert
        Assert.True(quantity.IsValid);
        Assert.Equal(input, quantity.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita QuantityValueObject -> int deve retornar valor interno")]
    public void ImplicitConversion_ToInt_ShouldReturnInternalValue()
    {
        // Arrange
        var quantity = QuantityValueObject.Build(8);

        // Act
        int result = quantity;

        // Assert
        Assert.Equal(8, result);
    }

    [Fact(DisplayName = "UnsafeBuildFromTrustedDatabaseRecord deve retornar objeto válido sem validação")]
    public void UnsafeBuildFromTrustedDatabaseRecord_ShouldReturnValidObject()
    {
        // Arrange
        var input = 100;

        // Act
        var quantity = QuantityValueObject.UnsafeBuildFromTrustedDatabaseRecord(input);

        // Assert
        Assert.True(quantity.IsValid);
        Assert.Equal(input, quantity.GetValue());
    }
}

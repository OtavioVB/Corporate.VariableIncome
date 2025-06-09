using Corporate.VariableIncome.Domain.Exceptions;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.UnitTests.Domain.ValueObjects;

public sealed class PriceValueObjectValidationTests
{
    [Fact(DisplayName = "Build() com valor igual a zero deve retornar objeto inválido")]
    public void Build_WithZero_ShouldReturnInvalid()
    {
        // Arrange
        var input = 0m;

        // Act
        var result = PriceValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com valor negativo deve retornar objeto inválido")]
    public void Build_WithNegativeValue_ShouldReturnInvalid()
    {
        // Arrange
        var input = -10.00m;

        // Act
        var result = PriceValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory(DisplayName = "Build() com mais de 2 casas decimais deve retornar objeto inválido")]
    [InlineData(1.123)]
    [InlineData(10.9999)]
    [InlineData(99.8765)]
    public void Build_WithMoreThanTwoDecimalPlaces_ShouldReturnInvalid(decimal input)
    {
        // Act
        var result = PriceValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory(DisplayName = "Build() com valor válido deve retornar objeto válido")]
    [InlineData(0.01)]
    [InlineData(1.00)]
    [InlineData(10.55)]
    [InlineData(999999.99)]
    public void Build_WithValidValue_ShouldReturnValidObject(decimal input)
    {
        // Act
        var result = PriceValueObject.Build(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(input, result.GetValue());
    }

    [Fact(DisplayName = "GetValue() em objeto inválido deve lançar ValueObjectException")]
    public void GetValue_WhenInvalid_ShouldThrowException()
    {
        // Arrange
        var result = PriceValueObject.Build(0m);

        // Act & Assert
        Assert.Throws<ValueObjectException>(() => result.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita decimal -> PriceValueObject deve funcionar")]
    public void ImplicitConversion_FromDecimal_ShouldReturnValid()
    {
        // Arrange
        decimal input = 12.50m;

        // Act
        PriceValueObject price = input;

        // Assert
        Assert.True(price.IsValid);
        Assert.Equal(input, price.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita PriceValueObject -> decimal deve retornar valor interno")]
    public void ImplicitConversion_ToDecimal_ShouldReturnValue()
    {
        // Arrange
        var price = PriceValueObject.Build(7.77m);

        // Act
        decimal result = price;

        // Assert
        Assert.Equal(7.77m, result);
    }

    [Fact(DisplayName = "UnsafeBuildFromTrustedDatabaseRecord deve retornar objeto válido sem validação")]
    public void UnsafeBuildFromTrustedDatabaseRecord_ShouldReturnValidObject()
    {
        // Arrange
        var input = 123.45m;

        // Act
        var result = PriceValueObject.UnsafeBuildFromTrustedDatabaseRecord(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(input, result.GetValue());
    }
}

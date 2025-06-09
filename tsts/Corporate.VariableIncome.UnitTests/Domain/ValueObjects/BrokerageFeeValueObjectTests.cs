using Corporate.VariableIncome.Domain.Exceptions;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.UnitTests.Domain.ValueObjects;

public sealed class BrokerageFeeValueObjectTests
{
    [Fact(DisplayName = "Build() com valor nulo ou zero deve retornar objeto inválido")]
    public void Build_WithZero_ShouldReturnInvalid()
    {
        // Arrange
        decimal input = 0m;

        // Act
        var result = BrokerageFeeValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com valor acima de 1 deve retornar objeto inválido")]
    public void Build_WithGreaterThanOne_ShouldReturnInvalid()
    {
        // Arrange
        decimal input = 1.01m;

        // Act
        var result = BrokerageFeeValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory(DisplayName = "Build() com valor válido e 2 casas decimais deve retornar objeto válido")]
    [InlineData(0.01)]
    [InlineData(0.25)]
    [InlineData(0.75)]
    [InlineData(1.00)]
    public void Build_WithValidPercentage_ShouldReturnValidObject(decimal input)
    {
        // Act
        var result = BrokerageFeeValueObject.Build(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(input, result.GetValue());
    }

    [Fact(DisplayName = "GetValue() em objeto inválido deve lançar ValueObjectException")]
    public void GetValue_WhenInvalid_ShouldThrowException()
    {
        // Arrange
        decimal input = 0m;
        var obj = BrokerageFeeValueObject.Build(input);

        // Assert
        Assert.Throws<ValueObjectException>(() => obj.GetValue());
    }

    [Fact(DisplayName = "GetValueAsPercentageString() deve retornar valor formatado corretamente")]
    public void GetValueAsPercentageString_ShouldReturnFormattedString()
    {
        // Arrange
        decimal input = 0.89m;
        var obj = BrokerageFeeValueObject.Build(input);

        // Act
        var result = obj.GetValueAsPercentageString();

        // Assert
        Assert.Equal("89%", result);
    }

    [Fact(DisplayName = "Conversão implícita decimal -> BrokerageFeeValueObject deve funcionar")]
    public void ImplicitConversion_FromDecimal_ShouldReturnValidObject()
    {
        // Arrange
        decimal input = 0.45m;

        // Act
        BrokerageFeeValueObject obj = input;

        // Assert
        Assert.True(obj.IsValid);
        Assert.Equal(input, obj.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita BrokerageFeeValueObject -> decimal deve funcionar")]
    public void ImplicitConversion_ToDecimal_ShouldReturnDecimalValue()
    {
        // Arrange
        var obj = BrokerageFeeValueObject.Build(0.12m);

        // Act
        decimal result = obj;

        // Assert
        Assert.Equal(0.12m, result);
    }

    [Fact(DisplayName = "UnsafeBuildFromTrustedDatabaseRecord deve retornar objeto válido sem validação")]
    public void UnsafeBuildFromTrustedDatabaseRecord_ShouldBypassValidation()
    {
        // Arrange
        decimal raw = 0.999999m;

        // Act
        var obj = BrokerageFeeValueObject.UnsafeBuildFromTrustedDatabaseRecord(raw);

        // Assert
        Assert.True(obj.IsValid);
        Assert.Equal(raw, obj.GetValue());
    }
}
using Corporate.VariableIncome.Domain.Exceptions;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.UnitTests.Domain.ValueObjects;

public sealed class ProftAndLossValueObjectValidationTests
{
    [Fact(DisplayName = "Build() com valor positivo deve retornar objeto válido")]
    public void Build_WithPositiveValue_ShouldBeValid()
    {
        // Arrange
        decimal input = 0.87m;

        // Act
        var result = ProftAndLossValueObject.Build(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(input, result.GetValue());
    }

    [Fact(DisplayName = "Build() com valor negativo deve retornar objeto válido")]
    public void Build_WithNegativeValue_ShouldBeValid()
    {
        // Arrange
        decimal input = -0.1543m;

        // Act
        var result = ProftAndLossValueObject.Build(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(input, result.GetValue());
    }

    [Fact(DisplayName = "Build() com valor zero deve retornar objeto válido")]
    public void Build_WithZero_ShouldBeValid()
    {
        // Arrange
        decimal input = 0m;

        // Act
        var result = ProftAndLossValueObject.Build(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(0m, result.GetValue());
    }

    [Fact(DisplayName = "UnsafeBuildFromTrustedDatabaseRecord deve criar objeto válido")]
    public void UnsafeBuildFromTrustedDatabaseRecord_ShouldBeValid()
    {
        // Arrange
        decimal input = 1.23m;

        // Act
        var result = ProftAndLossValueObject.UnsafeBuildFromTrustedDatabaseRecord(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(input, result.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita decimal -> ProftAndLossValueObject deve funcionar")]
    public void ImplicitConversion_FromDecimal_ShouldWork()
    {
        // Arrange
        decimal input = 0.42m;

        // Act
        ProftAndLossValueObject result = input;

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(input, result.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita ProftAndLossValueObject -> decimal deve funcionar")]
    public void ImplicitConversion_ToDecimal_ShouldWork()
    {
        // Arrange
        decimal input = 0.42m;
        var vo = ProftAndLossValueObject.Build(input);

        // Act
        decimal result = vo;

        // Assert
        Assert.Equal(input, result);
    }

    [Theory(DisplayName = "GetFormattedPercentage() deve formatar corretamente")]
    [InlineData(0.87, "87,00%")]
    [InlineData(-0.1543, "-15,43%")]
    [InlineData(0, "0,00%")]
    [InlineData(0.019, "1,90%")]
    public void GetFormattedPercentage_ShouldFormatCorrectly(decimal input, string expected)
    {
        // Arrange
        var vo = ProftAndLossValueObject.Build(input);

        // Act
        var result = vo.GetFormattedPercentage();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact(DisplayName = "GetValue() deve lançar exceção se objeto for inválido")]
    public void GetValue_WhenInvalid_ShouldThrowException()
    {
        // Arrange
        ProftAndLossValueObject obj = default;

        // Act & Assert
        Assert.Throws<ValueObjectException>(() => obj.GetValue());
    }
}

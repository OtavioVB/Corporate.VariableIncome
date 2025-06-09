using Corporate.VariableIncome.Domain.Exceptions;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.UnitTests.Domain.ValueObjects;

public sealed class PageValueObjectValidationTests
{
    [Fact(DisplayName = "Build() com valor válido deve retornar Page válido")]
    public void Build_WithValidValue_ShouldReturnValidPage()
    {
        // Arrange
        var input = 1;

        // Act
        var result = PageValueObject.Build(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(1, result.GetValue());
    }

    [Theory(DisplayName = "Build() com valor inválido (null, 0, negativo) deve retornar Page inválido")]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(null)]
    public void Build_WithInvalidValues_ShouldReturnInvalidPage(int? input)
    {
        // Arrange

        // Act
        var result = PageValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "GetValue() em Page inválido deve lançar ValueObjectException")]
    public void GetValue_WhenPageIsInvalid_ShouldThrowException()
    {
        // Arrange
        var input = 0;
        var result = PageValueObject.Build(input);

        // Act & Assert
        Assert.Throws<ValueObjectException>(() => result.GetValue());
    }

    [Fact(DisplayName = "GetValueAsString() deve retornar representação textual")]
    public void GetValueAsString_ShouldReturnStringRepresentation()
    {
        // Arrange
        var input = 5;
        var result = PageValueObject.Build(input);

        // Act
        var text = result.GetValueAsString();

        // Assert
        Assert.Equal("5", text);
    }

    [Fact(DisplayName = "Conversão implícita de int para PageValueObject deve funcionar corretamente")]
    public void ImplicitConversion_FromInt_ShouldCreateValidPage()
    {
        // Arrange
        int input = 3;

        // Act
        PageValueObject page = input;

        // Assert
        Assert.True(page.IsValid);
        Assert.Equal(input, page.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita de PageValueObject para int deve retornar valor interno")]
    public void ImplicitConversion_ToInt_ShouldReturnValue()
    {
        // Arrange
        var input = 7;
        var page = PageValueObject.Build(input);

        // Act
        int result = page;

        // Assert
        Assert.Equal(input, result);
    }
}
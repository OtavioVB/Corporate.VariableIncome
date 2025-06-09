using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Enumerators;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.ValueObjects;
using Corporate.VariableIncome.Domain.Exceptions;

namespace Corporate.VariableIncome.UnitTests.Domain.ValueObjects;


public sealed class TypeOperationValueObjectValidationTests
{
    [Fact(DisplayName = "Build() com enum válido deve retornar objeto válido")]
    public void Build_WithValidEnum_ShouldReturnValidObject()
    {
        // Arrange
        var input = EnumTypeOperation.BUY;

        // Act
        var result = TypeOperationValueObject.Build(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(EnumTypeOperation.BUY, result.GetValue());
    }

    [Fact(DisplayName = "Build() com enum inválido deve retornar objeto inválido")]
    public void Build_WithInvalidEnum_ShouldReturnInvalidObject()
    {
        // Arrange
        var invalid = (EnumTypeOperation)999;

        // Act
        var result = TypeOperationValueObject.Build(invalid);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory(DisplayName = "Build() com string válida deve retornar objeto válido")]
    [InlineData("BUY")]
    [InlineData("buy")]
    [InlineData("SeLL")]
    public void Build_WithValidString_ShouldReturnValid(string input)
    {
        // Act
        var result = TypeOperationValueObject.Build(input);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory(DisplayName = "Build() com string inválida deve retornar objeto inválido")]
    [InlineData("transfer")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void Build_WithInvalidString_ShouldReturnInvalid(string? input)
    {
        // Act
        var result = TypeOperationValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "GetValue() em objeto inválido deve lançar ValueObjectException")]
    public void GetValue_WhenInvalid_ShouldThrowException()
    {
        // Arrange
        var obj = TypeOperationValueObject.Build("");

        // Act & Assert
        Assert.Throws<ValueObjectException>(() => obj.GetValue());
    }

    [Fact(DisplayName = "GetValueAsString() deve retornar o valor como string")]
    public void GetValueAsString_ShouldReturnEnumName()
    {
        // Arrange
        var obj = TypeOperationValueObject.Build(EnumTypeOperation.SELL);

        // Act
        var result = obj.GetValueAsString();

        // Assert
        Assert.Equal("SELL", result);
    }

    [Fact(DisplayName = "Conversão implícita EnumTypeOperation -> TypeOperationValueObject deve funcionar")]
    public void ImplicitConversion_FromEnum_ShouldReturnValidObject()
    {
        // Arrange
        EnumTypeOperation value = EnumTypeOperation.BUY;

        // Act
        TypeOperationValueObject obj = value;

        // Assert
        Assert.True(obj.IsValid);
        Assert.Equal(value, obj.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita string -> TypeOperationValueObject deve funcionar")]
    public void ImplicitConversion_FromString_ShouldReturnValidObject()
    {
        // Arrange
        string input = "BUY";

        // Act
        TypeOperationValueObject obj = input;

        // Assert
        Assert.True(obj.IsValid);
        Assert.Equal(EnumTypeOperation.BUY, obj.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita TypeOperationValueObject -> EnumTypeOperation deve retornar enum")]
    public void ImplicitConversion_ToEnum_ShouldReturnEnumValue()
    {
        // Arrange
        var obj = TypeOperationValueObject.Build(EnumTypeOperation.SELL);

        // Act
        EnumTypeOperation result = obj;

        // Assert
        Assert.Equal(EnumTypeOperation.SELL, result);
    }

    [Fact(DisplayName = "Conversão implícita TypeOperationValueObject -> string deve retornar nome do enum")]
    public void ImplicitConversion_ToString_ShouldReturnEnumName()
    {
        // Arrange
        var obj = TypeOperationValueObject.Build(EnumTypeOperation.SELL);

        // Act
        string result = obj;

        // Assert
        Assert.Equal("SELL", result);
    }

    [Fact(DisplayName = "UnsafeBuildFromTrustedDatabaseRecord deve retornar objeto válido sem validação")]
    public void UnsafeBuildFromTrustedDatabaseRecord_ShouldReturnValid()
    {
        // Arrange
        var raw = EnumTypeOperation.BUY;

        // Act
        var obj = TypeOperationValueObject.UnsafeBuildFromTrustedDatabaseRecord(raw);

        // Assert
        Assert.True(obj.IsValid);
        Assert.Equal(raw, obj.GetValue());
    }

    [Fact(DisplayName = "Propriedade estática BUY deve retornar objeto válido")]
    public void StaticProperty_BUY_ShouldReturnValid()
    {
        // Act
        var result = TypeOperationValueObject.BUY;

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(EnumTypeOperation.BUY, result.GetValue());
    }

    [Fact(DisplayName = "Propriedade estática SELL deve retornar objeto válido")]
    public void StaticProperty_SELL_ShouldReturnValid()
    {
        // Act
        var result = TypeOperationValueObject.SELL;

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(EnumTypeOperation.SELL, result.GetValue());
    }
}
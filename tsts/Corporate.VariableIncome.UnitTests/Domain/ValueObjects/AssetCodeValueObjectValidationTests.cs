using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.ValueObjects;
using Corporate.VariableIncome.Domain.Exceptions;

namespace Corporate.VariableIncome.UnitTests.Domain.ValueObjects;

public sealed class AssetCodeValueObjectValidationTests
{
    [Fact(DisplayName = "Build() com valor nulo deve retornar objeto inválido")]
    public void Build_WithNull_ShouldReturnInvalid()
    {
        // Arrange
        string? input = null;

        // Act
        var result = AssetCodeValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com string vazia deve retornar objeto inválido")]
    public void Build_WithEmpty_ShouldReturnInvalid()
    {
        // Arrange
        var input = "";

        // Act
        var result = AssetCodeValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com espaços em branco deve retornar objeto inválido")]
    public void Build_WithWhiteSpace_ShouldReturnInvalid()
    {
        // Arrange
        var input = "     ";

        // Act
        var result = AssetCodeValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com mais de 10 caracteres deve retornar objeto inválido")]
    public void Build_WithMoreThanMaxLength_ShouldReturnInvalid()
    {
        // Arrange
        var input = new string('X', AssetCodeValueObject.MAX_LENGTH + 1);

        // Act
        var result = AssetCodeValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com código válido deve retornar objeto válido com letras maiúsculas")]
    public void Build_WithValidCode_ShouldReturnUpperCasedObject()
    {
        // Arrange
        var input = "itsa3";

        // Act
        var result = AssetCodeValueObject.Build(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal("ITSA3", result.GetValue());
    }

    [Fact(DisplayName = "GetValue() em objeto inválido deve lançar ValueObjectException")]
    public void GetValue_WhenInvalid_ShouldThrowException()
    {
        // Arrange
        var obj = AssetCodeValueObject.Build("");

        // Act & Assert
        Assert.Throws<ValueObjectException>(() => obj.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita string -> AssetCodeValueObject deve funcionar")]
    public void ImplicitConversion_FromString_ShouldReturnValidObject()
    {
        // Arrange
        string input = "petr4";

        // Act
        AssetCodeValueObject obj = input;

        // Assert
        Assert.True(obj.IsValid);
        Assert.Equal("PETR4", obj.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita AssetCodeValueObject -> string deve retornar valor interno")]
    public void ImplicitConversion_ToString_ShouldReturnInternalValue()
    {
        // Arrange
        var obj = AssetCodeValueObject.Build("abcd11");

        // Act
        string value = obj;

        // Assert
        Assert.Equal("ABCD11", value);
    }

    [Fact(DisplayName = "UnsafeBuildFromTrustedDatabaseRecord deve retornar objeto válido sem validação")]
    public void UnsafeBuildFromTrustedDatabaseRecord_ShouldBypassValidation()
    {
        // Arrange
        var raw = "kdif11";

        // Act
        var obj = AssetCodeValueObject.UnsafeBuildFromTrustedDatabaseRecord(raw);

        // Assert
        Assert.True(obj.IsValid);
        Assert.Equal(raw, obj.GetValue());
    }
}

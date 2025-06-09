using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.ValueObjects;
using Corporate.VariableIncome.Domain.Exceptions;

namespace Corporate.VariableIncome.UnitTests.Domain.ValueObjects;

public sealed class AssetNameValueObjectTests
{
    [Fact(DisplayName = "Build() com valor nulo deve retornar objeto inválido")]
    public void Build_WithNull_ShouldReturnInvalid()
    {
        // Arrange
        string? input = null;

        // Act
        var result = AssetNameValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com string vazia deve retornar objeto inválido")]
    public void Build_WithEmptyString_ShouldReturnInvalid()
    {
        // Arrange
        var input = "";

        // Act
        var result = AssetNameValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com espaços em branco deve retornar objeto inválido")]
    public void Build_WithWhiteSpace_ShouldReturnInvalid()
    {
        // Arrange
        var input = "     ";

        // Act
        var result = AssetNameValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com mais de 32 caracteres deve retornar objeto inválido")]
    public void Build_WithMoreThanMaxLength_ShouldReturnInvalid()
    {
        // Arrange
        var input = new string('A', AssetNameValueObject.MAX_LENGTH + 1);

        // Act
        var result = AssetNameValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com nome válido deve retornar objeto válido")]
    public void Build_WithValidName_ShouldReturnValid()
    {
        // Arrange
        var input = "Fundo XP Multimercado";

        // Act
        var result = AssetNameValueObject.Build(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(input, result.GetValue());
    }

    [Fact(DisplayName = "GetValue() em objeto inválido deve lançar ValueObjectException")]
    public void GetValue_WhenInvalid_ShouldThrowException()
    {
        // Arrange
        var result = AssetNameValueObject.Build("  ");

        // Act & Assert
        Assert.Throws<ValueObjectException>(() => result.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita string -> AssetNameValueObject deve funcionar")]
    public void ImplicitConversion_FromString_ShouldReturnValidObject()
    {
        // Arrange
        string input = "Tesouro Selic";

        // Act
        AssetNameValueObject obj = input;

        // Assert
        Assert.True(obj.IsValid);
        Assert.Equal(input, obj.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita AssetNameValueObject -> string deve retornar valor")]
    public void ImplicitConversion_ToString_ShouldReturnValue()
    {
        // Arrange
        var input = "CDB Banco XP";
        var obj = AssetNameValueObject.Build(input);

        // Act
        string result = obj;

        // Assert
        Assert.Equal(input, result);
    }

    [Fact(DisplayName = "UnsafeBuildFromTrustedDatabaseRecord deve retornar objeto válido sem validação")]
    public void UnsafeBuildFromTrustedDatabaseRecord_ShouldReturnValid()
    {
        // Arrange
        var raw = "Fundo Imobiliário XPTO";

        // Act
        var obj = AssetNameValueObject.UnsafeBuildFromTrustedDatabaseRecord(raw);

        // Assert
        Assert.True(obj.IsValid);
        Assert.Equal(raw, obj.GetValue());
    }
}
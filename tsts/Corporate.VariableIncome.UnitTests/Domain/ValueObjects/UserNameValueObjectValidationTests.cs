using Corporate.VariableIncome.Domain.BoundedContexts.UserContext.ValueObjects;
using Corporate.VariableIncome.Domain.Exceptions;

namespace Corporate.VariableIncome.UnitTests.Domain.ValueObjects;

public sealed class UserNameValueObjectTests
{
    [Fact(DisplayName = "Build() com nome nulo deve retornar objeto inválido")]
    public void Build_WithNullValue_ShouldReturnInvalid()
    {
        // Arrange
        string? name = null;

        // Act
        var result = UserNameValueObject.Build(name);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com nome em branco deve retornar objeto inválido")]
    public void Build_WithWhiteSpaceValue_ShouldReturnInvalid()
    {
        // Arrange
        var name = "     ";

        // Act
        var result = UserNameValueObject.Build(name);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com nome maior que MAX_LENGTH deve retornar objeto inválido")]
    public void Build_WithNameExceedingMaxLength_ShouldReturnInvalid()
    {
        // Arrange
        var name = new string('a', UserNameValueObject.MAX_LENGTH + 1);

        // Act
        var result = UserNameValueObject.Build(name);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com nome válido deve retornar objeto válido com formatação correta")]
    public void Build_WithValidName_ShouldReturnFormattedName()
    {
        // Arrange
        var rawName = "joão da silva";

        // Act
        var result = UserNameValueObject.Build(rawName);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal("João Da Silva", result.GetValue());
    }

    [Fact(DisplayName = "GetValue() em objeto inválido deve lançar ValueObjectException")]
    public void GetValue_WhenInvalid_ShouldThrowException()
    {
        // Arrange
        var name = "";

        // Act
        var result = UserNameValueObject.Build(name);

        // Assert
        Assert.Throws<ValueObjectException>(() => result.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita string -> UserNameValueObject deve retornar objeto válido")]
    public void ImplicitConversion_FromString_ShouldReturnValidObject()
    {
        // Arrange
        string name = "joão da silva";

        // Act
        UserNameValueObject valueObject = name;

        // Assert
        Assert.True(valueObject.IsValid);
        Assert.Equal("João Da Silva", valueObject.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita UserNameValueObject -> string deve retornar valor interno")]
    public void ImplicitConversion_ToString_ShouldReturnInternalValue()
    {
        // Arrange
        var input = "joão da silva";
        var valueObject = UserNameValueObject.Build(input);

        // Act
        string value = valueObject;

        // Assert
        Assert.Equal("João Da Silva", value);
    }

    [Fact(DisplayName = "UnsafeBuildFromTrustedDatabaseRecord deve retornar objeto válido com valor bruto")]
    public void UnsafeBuildFromTrustedDatabaseRecord_ShouldReturnValidUnformattedValue()
    {
        // Arrange
        var raw = "joao minusculo sem formatacao";

        // Act
        var result = UserNameValueObject.UnsafeBuildFromTrustedDatabaseRecord(raw);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(raw, result.GetValue());
    }
}

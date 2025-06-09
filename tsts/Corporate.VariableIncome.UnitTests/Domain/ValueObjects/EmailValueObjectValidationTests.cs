using Corporate.VariableIncome.Domain.Exceptions;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.UnitTests.Domain.ValueObjects;

public sealed class EmailValueObjectTests
{
    [Fact(DisplayName = "Build() com valor nulo deve retornar objeto inválido")]
    public void Build_WithNull_ShouldReturnInvalid()
    {
        // Arrange
        string? input = null;

        // Act
        var result = EmailValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com string em branco deve retornar objeto inválido")]
    public void Build_WithWhiteSpace_ShouldReturnInvalid()
    {
        // Arrange
        var input = "   ";

        // Act
        var result = EmailValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com string acima de MAX_LENGTH deve retornar objeto inválido")]
    public void Build_WithTooLongEmail_ShouldReturnInvalid()
    {
        // Arrange
        var localPart = new string('a', 250); // 246 + "@x.com" = 255+
        var input = $"{localPart}@x.com";

        // Act
        var result = EmailValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com formato inválido deve retornar objeto inválido")]
    public void Build_WithInvalidFormat_ShouldReturnInvalid()
    {
        // Arrange
        var input = "invalid-email-format";

        // Act
        var result = EmailValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com e-mail válido deve retornar objeto válido")]
    public void Build_WithValidEmail_ShouldReturnValidObject()
    {
        // Arrange
        var input = "joao.silva@email.com";

        // Act
        var result = EmailValueObject.Build(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(input, result.GetValue());
    }

    [Fact(DisplayName = "GetValue() em e-mail inválido deve lançar ValueObjectException")]
    public void GetValue_WhenInvalid_ShouldThrowException()
    {
        // Arrange
        var input = "email.invalido";

        // Act
        var result = EmailValueObject.Build(input);

        // Assert
        Assert.Throws<ValueObjectException>(() => result.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita string -> EmailValueObject deve retornar objeto válido")]
    public void ImplicitConversion_FromString_ShouldReturnValidObject()
    {
        // Arrange
        string input = "otavio@dominio.com";

        // Act
        EmailValueObject email = input;

        // Assert
        Assert.True(email.IsValid);
        Assert.Equal(input, email.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita EmailValueObject -> string deve retornar valor interno")]
    public void ImplicitConversion_ToString_ShouldReturnInternalValue()
    {
        // Arrange
        var input = "otavio@empresa.com";
        var email = EmailValueObject.Build(input);

        // Act
        string result = email;

        // Assert
        Assert.Equal(input, result);
    }

    [Fact(DisplayName = "UnsafeBuildFromTrustedDatabaseRecord deve retornar objeto válido com valor bruto")]
    public void UnsafeBuildFromTrustedDatabaseRecord_ShouldReturnValidWithoutValidation()
    {
        // Arrange
        var raw = "persistido@banco.com";

        // Act
        var result = EmailValueObject.UnsafeBuildFromTrustedDatabaseRecord(raw);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(raw, result.GetValue());
    }
}
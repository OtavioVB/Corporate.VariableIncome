using Corporate.VariableIncome.Domain.Exceptions;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.UnitTests.Domain.ValueObjects;

public sealed class IdValueObjectTests
{
    [Fact(DisplayName = "Build() com null deve retornar Id válido com valor gerado")]
    public void Build_WithNull_ReturnsValidIdWithGeneratedGuid()
    {
        // Arrange

        // Act
        var id = IdValueObject.Build();

        // Assert
        Assert.True(id.IsValid);
        Assert.NotEqual(Guid.Empty, id.GetValue());
    }

    [Fact(DisplayName = "Build() com Guid válido deve retornar Id válido com mesmo valor")]
    public void Build_WithValidGuid_ReturnsValidId()
    {
        // Arrange
        var input = Guid.NewGuid();

        // Act
        var id = IdValueObject.Build(input);

        // Assert
        Assert.True(id.IsValid);
        Assert.Equal(input, id.GetValue());
    }

    [Fact(DisplayName = "Build() com Guid.Empty deve retornar Id inválido")]
    public void Build_WithEmptyGuid_ReturnsInvalidId()
    {
        // Arrange
        var input = Guid.Empty;

        // Act
        var id = IdValueObject.Build(input);
        
        // Assert
        Assert.False(id.IsValid);
    }

    [Fact(DisplayName = "GetValue() em Id inválido deve lançar ValueObjectException")]
    public void GetValue_WhenInvalid_ThrowsException()
    {
        // Arrange
        var input = Guid.Empty;

        // Act
        var id = IdValueObject.Build(input);

        // Assert
        Assert.Throws<ValueObjectException>(() => id.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita Guid -> IdValueObject deve criar instância válida")]
    public void ImplicitConversion_FromGuid_ReturnsValidId()
    {
        // Arrange
        Guid guid = Guid.NewGuid();
        
        // Act
        IdValueObject id = guid;

        // Assert
        Assert.True(id.IsValid);
        Assert.Equal(guid, id.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita IdValueObject -> Guid deve retornar valor interno")]
    public void ImplicitConversion_ToGuid_ReturnsInternalGuid()
    {
        // Arrange
        var input = Guid.NewGuid();
        var id = IdValueObject.Build(input);

        // Act
        Guid guid = id;

        // Assert
        Assert.Equal(input, guid);
    }

    [Fact(DisplayName = "UnsafeBuildFromTrustedDatabaseRecord deve construir Id válido sem validação")]
    public void UnsafeBuildFromTrustedDatabaseRecord_ReturnsValidId()
    {
        // Arrange
        var input = Guid.NewGuid();

        // Act
        var id = IdValueObject.UnsafeBuildFromTrustedDatabaseRecord(input);

        // Assert
        Assert.True(id.IsValid);
        Assert.Equal(input, id.GetValue());
    }

    [Fact(DisplayName = "GetValueAsString deve retornar o Guid em formato string")]
    public void GetValueAsString_ReturnsGuidString()
    {
        // Arrange
        var input = Guid.NewGuid();
        var id = IdValueObject.Build(input);

        // Act
        var stringValue = id.GetValueAsString();
        
        // Assert
        Assert.Equal(input.ToString(), stringValue);
    }
}

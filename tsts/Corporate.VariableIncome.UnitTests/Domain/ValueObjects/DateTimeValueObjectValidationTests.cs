using Corporate.VariableIncome.Domain.Exceptions;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.UnitTests.Domain.ValueObjects;

public sealed class DateTimeValueObjectValidationTests
{
    [Fact(DisplayName = "Build() com DateTime nulo deve retornar objeto com valor atual (UTC)")]
    public void Build_WithNull_ShouldUseUtcNow()
    {
        // Arrange
        DateTime? input = null;
        var before = DateTime.UtcNow;

        // Act
        var result = DateTimeValueObject.Build(input);
        var after = DateTime.UtcNow;

        // Assert
        Assert.True(result.IsValid);
        Assert.True(result.GetValue() >= before && result.GetValue() <= after);
    }

    [Theory(DisplayName = "Build() com DateTime inválido deve retornar objeto inválido")]
    [InlineData("0001-01-01")] // MinValue
    public void Build_WithInvalidDate_ShouldReturnInvalid(DateTime input)
    {
        // Act
        var result = DateTimeValueObject.Build(input);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Build() com DateTime válido deve retornar objeto válido")]
    public void Build_WithValidDate_ShouldReturnValid()
    {
        // Arrange
        var input = new DateTime(2023, 12, 31, 14, 30, 0, DateTimeKind.Utc);

        // Act
        var result = DateTimeValueObject.Build(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(input, result.GetValue());
    }

    [Fact(DisplayName = "GetValue() em objeto inválido deve lançar ValueObjectException")]
    public void GetValue_WhenInvalid_ShouldThrowException()
    {
        // Arrange
        var obj = DateTimeValueObject.Build(DateTime.MinValue);

        // Act & Assert
        Assert.Throws<ValueObjectException>(() => obj.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita DateTime? -> DateTimeValueObject deve funcionar")]
    public void ImplicitConversion_FromNullableDateTime_ShouldReturnValidObject()
    {
        // Arrange
        DateTime? input = new(2022, 06, 01, 10, 0, 0);

        // Act
        DateTimeValueObject obj = input;

        // Assert
        Assert.True(obj.IsValid);
        Assert.Equal(input, obj.GetValue());
    }

    [Fact(DisplayName = "Conversão implícita DateTimeValueObject -> DateTime deve retornar valor interno")]
    public void ImplicitConversion_ToDateTime_ShouldReturnInternalValue()
    {
        // Arrange
        var input = DateTime.UtcNow;
        var obj = DateTimeValueObject.Build(input);

        // Act
        DateTime result = obj;

        // Assert
        Assert.Equal(input, result);
    }

    [Fact(DisplayName = "UnsafeBuildFromTrustedDatabaseRecord deve retornar objeto válido")]
    public void UnsafeBuildFromTrustedDatabaseRecord_ShouldReturnValid()
    {
        // Arrange
        var raw = new DateTime(2023, 01, 01, 12, 0, 0);

        // Act
        var result = DateTimeValueObject.UnsafeBuildFromTrustedDatabaseRecord(raw);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(raw, result.GetValue());
    }
}

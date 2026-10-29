using Castleguard.Api;

namespace Castleguard.Tests;

public class VaultItemValidatorTests
{
    [Theory]
    [InlineData("Castleguard")]
    [InlineData(" Castleguard With Spaces ")]
    [InlineData("64TestofmaxlenghtTestofmaxlenghtTestofmaxlenghtestofmaxlenght012")]
    public void ValidateName_WithValidInput_ReturnsTrue(string name)
    {
        // Arrange.
        var expected = new VaultItemValidator.ValidationResult(true);
        var result = VaultItemValidator.ValidateName(name);
        
        // Act & Assert.
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ValidateName_WithEmptyInput_ReturnsFalse(string? name)
    {
        // Arrange.
        var expected = new VaultItemValidator.ValidationResult(false, "Name must not be empty.");
        var result = VaultItemValidator.ValidateName(name);
        
        // Act & Assert.
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData("65TestofmaxlenghtTestofmaxlenghtTestofmaxlenghtestofmaxlenght0123")]
    public void ValidateName_WithInvalidInputLength_ReturnsFalse(string? name)
    {
        // Arrange.
        var expected = new VaultItemValidator.ValidationResult(false, "Name must not exceed 64 characters.");
        var result = VaultItemValidator.ValidateName(name);
        
        // Act & Assert.
        Assert.Equal(expected, result);
    }
}
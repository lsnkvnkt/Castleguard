using Castleguard.Api;

namespace Castleguard.Tests;

public class VaultServiceTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreateVaultItemAsync_WithEmptyInput_ThrowsValidationException(string? name)
    {
        // Arrange.
        var service = new TestVaultService();
        
        // Act.
        var action = new Func<Task>(async () => await service.CreateVaultItemAsync(name!));
        
        // Assert.
        var exception = await Assert.ThrowsAsync<ArgumentException>(action);
        Assert.Equal($"Name must not be empty.", exception.Message);
    }

    [Theory]
    [InlineData("65TestofmaxlenghtTestofmaxlenghtTestofmaxlenghtestofmaxlenght0123")]
    public async Task CreateVaultItemAsync_WithInvalidInputLength_ThrowsValidationException(string? name)
    {
        // Arrange.
        var service = new TestVaultService();
        
        // Act.
        var action = new Func<Task>(async () => await service.CreateVaultItemAsync(name!));
        
        // Assert.
        var exception = await Assert.ThrowsAsync<ArgumentException>(action);
        Assert.Equal($"Name must not exceed 64 characters.", exception.Message);
    }
}
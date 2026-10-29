using Castleguard.Api;

namespace Castleguard.Tests;

public class TestVaultServiceTests
{
    [Theory]
    [InlineData("Castleguard")]
    [InlineData(" Castleguard With Spaces ")]
    [InlineData("64TestofmaxlenghtTestofmaxlenghtTestofmaxlenghtestofmaxlenght012")]
    public async Task CreateVaultItemAsync_WithValidInput_ReturnsVaultItem(string name)
    {
        // Arrange.
        var service = new TestVaultService();
        
        // Act.
        var item = await service.CreateVaultItemAsync(name);
        var items = await service.GetVaultItemsAsync();
        
        // Assert.
        Assert.Equal(name.Trim(), item.Name);
        Assert.Contains(items, x => x.Name == item.Name && x.Id == item.Id);
    }

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
        Assert.Equal("Name must not be empty.", exception.Message);
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
        Assert.Equal("Name must not exceed 64 characters.", exception.Message);
    }
}
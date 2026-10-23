using Castleguard.Api;

namespace Castleguard.Tests;

public class TestVaultServiceTests
{
    [Theory]
    [InlineData("Castleguard")]
    [InlineData(" Castleguard With Spaces ")]
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
    public async Task CreateVaultItemAsync_WithInvalidInput_ThrowsValidationException(string? name)
    {
        // Arrange.
        var service = new TestVaultService();
        
        // Act.
        var action = new Func<Task>(async () => await service.CreateVaultItemAsync(name!));
        
        // Assert.
        var exception = await Assert.ThrowsAsync<ArgumentException>(action);
        Assert.Equal("The name is invalid. It must be a non-empty string with a maximum length of 64 characters.", exception.Message);
    }
}
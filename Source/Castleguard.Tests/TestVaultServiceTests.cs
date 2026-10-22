using Castleguard.Api;

namespace Castleguard.Tests;

public class TestVaultServiceTest
{
    [Theory]
    [InlineData("Castleguard")]
    public async Task CreateVaultItemAsync_WithValidInput_ReturnsVaultItem(string name)
    {
        // Arrange.
        var service = new TestVaultService();
        
        // Act.
        var item = await service.CreateVaultItemAsync(name);
        var items = await service.GetVaultItemsAsync();
        
        // Assert.
        Assert.Equal(name, item.Name);
        Assert.Contains(items, x => x.Name == item.Name && x.Id == item.Id);
    }
}
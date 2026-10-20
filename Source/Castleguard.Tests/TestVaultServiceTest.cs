using Castleguard.Api;

namespace Castleguard.Tests;

public class TestVaultServiceTest
{
    [Fact]
    public async Task CreateVaultItemAsync_AddsItemToList()
    {
        var service = new TestVaultService();
        const string name = "Castleguard";
        var item = await service.CreateVaultItemAsync(name);
        
        Assert.Equal(name, item.Name);
        
        var items = await service.GetVaultItemsAsync();
        
        Assert.Contains(items, x => x.Name == item.Name && x.Id == item.Id);
    }
}
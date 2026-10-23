using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Castleguard.Tests")]

namespace Castleguard.Api;

internal class TestVaultService : IVaultService
{
    private List<VaultItem> _vaultItems =
    [
        new(29, "Anna"),
        new(21, "Rose")
    ];

    public Task<List<VaultItem>> GetVaultItemsAsync()
        => Task.FromResult(_vaultItems);

    public Task<VaultItem?> GetVaultItemAsync(int id)
        => Task.FromResult(_vaultItems.Find(x => x.Id == id));

    public Task<VaultItem> CreateVaultItemAsync(string name)
    {
        if (!VaultItemEntity.IsValidName(name))
            throw new ArgumentException("The name is invalid. It must be a non-empty string with a maximum length of 64 characters.");
        
        var id = Random.Shared.Next();
        while (_vaultItems.Any(x => x.Id == id))
            id = Random.Shared.Next();

        var item = new VaultItem(id, name.Trim());
        _vaultItems.Add(item);
        return Task.FromResult(item);
    }

    public Task<VaultItem?> EditVaultItemNameAsync(int id, string newName)
    {
        if (!VaultItemEntity.IsValidName(newName))
            throw new ArgumentException("The name is invalid. It must be a non-empty string with a maximum length of 64 characters.");
        
        var index = _vaultItems.FindIndex(x => x.Id == id);
        if (index == -1)
            return Task.FromResult<VaultItem?>(null);
        
        var item = new VaultItem(id, newName.Trim());
        _vaultItems[index] = item;
        return Task.FromResult<VaultItem?>(item);
    }

    public Task<VaultItem?> DeleteVaultItemAsync(int id)
    {
        var item = _vaultItems.Find(x => x.Id == id);
        if (item is null)
            return Task.FromResult<VaultItem?>(null);
        
        _vaultItems.Remove(item);
        return Task.FromResult<VaultItem?>(item);
    }
}
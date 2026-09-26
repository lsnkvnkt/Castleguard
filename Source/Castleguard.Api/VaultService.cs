namespace Castleguard.Api;

internal class VaultService
{
    private List<VaultItem> _vaultItems =
    [
        new(0, "Steve"),
        new(1, "Michael")
    ];

    internal List<VaultItem> GetVaultItems()
        => _vaultItems;
    
    internal VaultItem? GetVaultItem(int id)
        => _vaultItems.Find(x => x.Id == id);
    
    internal VaultItem CreateVaultItem(string name)
    {
        var id = Random.Shared.Next();
        while (_vaultItems.Any(x => x.Id == id))
            id = Random.Shared.Next();

        var item = new VaultItem(id, name);
        _vaultItems.Add(item);
        return item;
    }

    internal VaultItem? EditVaultItemName(int id, string newName)
    {
        var index = _vaultItems.FindIndex(x => x.Id == id);
        if (index == -1)
            return null;
        
        var item = new VaultItem(id, newName);
        _vaultItems[index] = item;
        return item;
    }

    internal VaultItem? DeleteVaultItem(int id)
    {
        var item = _vaultItems.Find(x => x.Id == id);
        if (item is null)
            return null;
        
        _vaultItems.Remove(item);
        return item;
    }
    
    internal record VaultItem(int Id, string Name);
}
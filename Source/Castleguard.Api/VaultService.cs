using Microsoft.EntityFrameworkCore;

namespace Castleguard.Api;

internal record VaultItem(int Id, string Name);

internal interface IVaultService
{
    Task<List<VaultItem>> GetVaultItemsAsync();
    Task<VaultItem?> GetVaultItemAsync(int id);
    Task<VaultItem> CreateVaultItemAsync(string name);
    Task<VaultItem?> EditVaultItemNameAsync(int id, string newName);
    Task<VaultItem?> DeleteVaultItemAsync(int id);
}

internal class VaultService(CastleguardDbContext dbContext) : IVaultService
{
    public async Task<List<VaultItem>> GetVaultItemsAsync()
    { 
        return await dbContext.VaultItems
            .Select(entity => new VaultItem(entity.Id, entity.Name))
            .ToListAsync();
    }

    public async Task<VaultItem?> GetVaultItemAsync(int id)
    {
        var entity = await dbContext.VaultItems.FindAsync(id);
        return entity is null ? null : new VaultItem(entity.Id, entity.Name);
    }

    public async Task<VaultItem> CreateVaultItemAsync(string name)
    {
        var entity = new VaultItemEntity() { Name = name };

        dbContext.VaultItems.Add(entity);
        await dbContext.SaveChangesAsync();

        return new VaultItem(entity.Id, name);
    }

    public async Task<VaultItem?> EditVaultItemNameAsync(int id, string newName)
    {
        var entity = await dbContext.VaultItems.FindAsync(id);
        if (entity is null)
            return null;
        
        entity.Name = newName;
        await dbContext.SaveChangesAsync();

        return new VaultItem(entity.Id, entity.Name);
    }

    public async Task<VaultItem?> DeleteVaultItemAsync(int id)
    {
        var entity = await dbContext.VaultItems.FindAsync(id);
        if (entity is null)
            return null;

        var deletedEntity = new VaultItem(entity.Id, entity.Name);
        
        dbContext.VaultItems.Remove(entity);
        await dbContext.SaveChangesAsync();
        
        return deletedEntity;
    }
}
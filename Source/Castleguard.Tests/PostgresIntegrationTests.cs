using Castleguard.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Castleguard.Tests;

public class PostgresIntegrationTests(PostgresDatabaseFixture fixture) : IClassFixture<PostgresDatabaseFixture>
{
    [Fact]
    public async Task CreateVaultItemAsync_WithValidInput_PersistsAndReturnsVaultItem()
    {
        int? createdItemId = null;
        try
        {
            var name = $"IntegrationTest_{Guid.NewGuid():N}";
            using (var scope = fixture.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IVaultService>();
                var item = await service.CreateVaultItemAsync(name);
                createdItemId = item.Id;
            }

            using (var scope = fixture.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CastleguardDbContext>();
                
                var entity = await dbContext.VaultItems.FindAsync([createdItemId], TestContext.Current.CancellationToken);
                Assert.NotNull(entity);
                Assert.Equal(name, entity.Name);
                Assert.True(entity.Id > 0);
            }
        }
        finally
        {
            if (createdItemId is { } id)
            {
                using var scope = fixture.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<CastleguardDbContext>();

                var entity = await dbContext.VaultItems.FindAsync([id], CancellationToken.None);
                if (entity != null)
                {
                    dbContext.VaultItems.Remove(entity);
                    await dbContext.SaveChangesAsync(CancellationToken.None);
                }
            }
        }
    }

    [Fact]
    public async Task EditVaultItemNameAsync_WithValidInput_UpdatesVaultItemName()
    {
        int? createdItemId = null;
        using (var scope = fixture.CreateScope())
        {
            var item = new VaultItemEntity() { Name = $"IntegrationTest_{Guid.NewGuid():N}" };
            
            var dbContext = scope.ServiceProvider.GetRequiredService<CastleguardDbContext>();
            dbContext.VaultItems.Add(item);
            await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);
            
            createdItemId = item.Id;
        }
        
        try
        {
            var newName = $"IntegrationTest_{Guid.NewGuid():N}";
            using (var scope = fixture.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IVaultService>();
                await service.EditVaultItemNameAsync(createdItemId.Value, newName);
            }

            using (var scope = fixture.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CastleguardDbContext>();

                var entity = await dbContext.VaultItems.FindAsync([createdItemId], TestContext.Current.CancellationToken);
                Assert.NotNull(entity);
                Assert.Equal(createdItemId, entity.Id);
                Assert.Equal(newName, entity.Name);
            }
        }
        finally
        {
            if (createdItemId is { } id)
            {
                using var scope = fixture.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<CastleguardDbContext>();

                var entity = await dbContext.VaultItems.FindAsync([id], CancellationToken.None);
                if (entity != null)
                {
                    dbContext.VaultItems.Remove(entity);
                    await dbContext.SaveChangesAsync(CancellationToken.None);
                }
            }
        }
    }
    
    [Fact]
    public void SmokeTest()
    {
        // Arrange.
        using var scope = fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CastleguardDbContext>();
        
        // Act & Assert.
        Assert.Equal("castleguard_test", context.Database.GetDbConnection().Database);
    }
}
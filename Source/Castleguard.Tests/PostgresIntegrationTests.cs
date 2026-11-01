using Castleguard.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Castleguard.Tests;

public class PostgresIntegrationTests(PostgresDatabaseFixture fixture) : IClassFixture<PostgresDatabaseFixture>
{
    [Fact]
    public async Task CreateVaultItemAsync_WithValidInput_PersistsAndReturnsVaultItem()
    {
        using var scope = fixture.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IVaultService>();

        var dbContext = scope.ServiceProvider.GetRequiredService<CastleguardDbContext>();
        VaultItemEntity? entity = null;
        try
        {
            var item = await service.CreateVaultItemAsync("CastleguardIntegrationTestUser");
            
            entity = await dbContext.VaultItems.FindAsync([item.Id], TestContext.Current.CancellationToken);
            Assert.NotNull(entity);
            Assert.Equal("CastleguardIntegrationTestUser", item.Name);
            Assert.True(item.Id > 0);
        }
        finally
        {
            if (entity != null)
            {
                dbContext.VaultItems.Remove(entity);
                await dbContext.SaveChangesAsync(CancellationToken.None);
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
using Microsoft.EntityFrameworkCore;

namespace Castleguard.Tests;

public class PostgresIntegrationTests(PostgresDatabaseFixture fixture) : IClassFixture<PostgresDatabaseFixture>
{
    [Fact]
    public async Task SmokeTest()
    {
        await using var dbContext = fixture.CreateDbContext();
        var items = await dbContext.VaultItems.ToListAsync();
    }
}
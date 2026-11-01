using Castleguard.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Castleguard.Tests;

public class PostgresIntegrationTests(PostgresDatabaseFixture fixture) : IClassFixture<PostgresDatabaseFixture>
{
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
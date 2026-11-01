using Castleguard.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Castleguard.Tests;

public class PostgresDatabaseFixture : IAsyncLifetime
{
    private const string UserSecretId = "c16b5f2a-234d-4857-8621-39d703a1181d";
    private const string ConnectionStringName = "CastleguardTest";
    private const string DatabaseName = "castleguard_test";

    private string ConnectionString { get; }
    private DbContextOptions<CastleguardDbContext> DbContextOptions { get; }
    private readonly ServiceProvider _serviceProvider;

    public CastleguardDbContext CreateDbContext() => new(DbContextOptions);
    
    public IServiceScope CreateScope() =>  _serviceProvider.CreateScope();

    public async ValueTask InitializeAsync()
    {
        await using var dbContext = CreateDbContext();
        if (dbContext.Database.GetDbConnection().Database != DatabaseName)
            throw new InvalidOperationException($"Expected database \"{DatabaseName}\", but found \"{dbContext.Database.GetDbConnection().Database}\".");
        
        await dbContext.Database.MigrateAsync();
    }
    
    public async ValueTask DisposeAsync()
    {
        await _serviceProvider.DisposeAsync();
        GC.SuppressFinalize(this);
    }
    
    public PostgresDatabaseFixture()
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddUserSecrets(UserSecretId);
        var configuration = configurationBuilder.Build();
        
        ConnectionString = configuration.GetConnectionString(ConnectionStringName)
                           ?? throw new InvalidOperationException("ConnectionString not found.");
        
        DbContextOptions = new DbContextOptionsBuilder<CastleguardDbContext>().UseNpgsql(ConnectionString).Options;

        var serviceProvider = new ServiceCollection();
        serviceProvider.AddScoped<IVaultService, VaultService>();
        serviceProvider.AddDbContext<CastleguardDbContext>(options => options.UseNpgsql(ConnectionString));
        _serviceProvider = serviceProvider.BuildServiceProvider();
    }
}
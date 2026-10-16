using Microsoft.EntityFrameworkCore;

namespace Castleguard.Api;

public class CastleguardDbContext(DbContextOptions<CastleguardDbContext> options) : DbContext(options)
{
    public DbSet<VaultItemEntity> VaultItems => Set<VaultItemEntity>();
}
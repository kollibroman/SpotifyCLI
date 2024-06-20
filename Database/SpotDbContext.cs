using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class SpotDbContext : DbContext
{
    public SpotDbContext(DbContextOptions<SpotDbContext> options) : base(options) 
    {
    }
    
    public virtual DbSet<ClientData> ClientData => Set<ClientData>();
    public virtual DbSet<Device> Device => Set<Device>();
    public virtual DbSet<Token> Token => Set<Token>();
    public virtual DbSet<UsrAccount> UsrAccount => Set<UsrAccount>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SpotDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Database;

public class SpotDbContext : DbContext
{
    public SpotDbContext()
    {
    }
    
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=spot.db");
    }
}
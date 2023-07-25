using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class SpotDbContext : DbContext
    {
        private readonly string _connstr = "SpotDb";

        public DbSet<AppDetails> AppDetails { get; set; }
        public DbSet<Device> Device { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<UsrAccount> UsrAccount { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseSqlite($"DataSource={_connstr}");
        }
    }
}

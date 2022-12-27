using Microsoft.EntityFrameworkCore;
using SpotifyCli.Db.Entities;


namespace SpotifyCli.Db
{
    public class SpotifyDbContext : DbContext
    {
        private readonly string _connstr = "Data Source=SpotDB";

        public DbSet<AppDetails> AppDetails { get; set; }
        public DbSet<Device> Device { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<UsrAccount> UsrAccount { get; set; }
        public DbSet<CurrentlyPlaying> CurrentlyPlaying { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseSqlite(_connstr);
        }
        
        
    }
}
using SpotifyCli.Db;

namespace SpotifyClientCli.Modules
{
    [Command(Description = "Logouts the user")]
    public class Logout : ISpotifyBase
    {
        private readonly ILogger<Logout> _logger;
        private readonly SpotifyDbContext _db;
        public Logout(ILogger<Logout> logger, SpotifyDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            var account = _db.UsrAccount.Where(i => i.Id == 1);
            _db.Remove(account);
            _logger.LogInformation("Successfully logged out!");
            await _db.SaveChangesAsync();
        }
    }
}
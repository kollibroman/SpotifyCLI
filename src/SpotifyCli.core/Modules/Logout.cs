namespace SpotifyClientCli.Modules
{
    [Command(Description = "Logouts the user")]
    public class Logout : ISpotifyBase
    {
        private readonly AppConfig _appconfig;
        private readonly ILogger<Logout> _logger;
        public Logout(ILogger<Logout> logger, AppConfig app)
        {
            _appconfig = app;
            _logger = logger;
        }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _appconfig.Token.AccessToken = null;
            _appconfig.Token.RefreshToken = null;
            _appconfig.Token.CreatedAt = null;
            _appconfig.Token.ExpiresIn = null;
            _appconfig.Token.TokenType = null;
            await _appconfig.SaveAsync();
            _logger.LogInformation("Successfully logged out!");
            await Task.CompletedTask;
        }
    }
}
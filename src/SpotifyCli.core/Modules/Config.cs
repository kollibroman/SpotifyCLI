namespace SpotifyCli.core.Modules
{
    [Command("config", Description = "Configure client id and client secret values")]
   public class Config : ISpotifyBase
    {
        private readonly AppConfig _appconfig;
        private readonly ILogger<Config> _logger;

        public Config(AppConfig appconfig, ILogger<Config> logger)
        {
            _appconfig = appconfig;
            _logger = logger;
        }

        [Option("--client-id")]
        public string? ClientId { get; set; }
        [Option("--client-secret")]
        public string? ClientSecret { get; set; }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _appconfig.App.ClientId = ClientId;
            _appconfig.App.ClientSecret = ClientSecret;
            await _appconfig.SaveAsync();
            _logger.LogInformation("Changed!");
        }
    }
}
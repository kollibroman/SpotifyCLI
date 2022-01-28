namespace SpotifyClientCli.Modules
{
    [Command(Name = "spotify")]
    [VersionOptionFromMember("-v")]
    [Subcommand(
        typeof(Login),
        typeof(Playlists),
        typeof(Player),
        typeof(Logout),
        typeof(Account),
        typeof(Library),
        typeof(Search)
    )]
    public class SpotifyCmd : ISpotifyBase
    {
        private readonly ILogger<SpotifyCmd> _logger;
        public SpotifyCmd(ILogger<SpotifyCmd> logger)
        {
            _logger = logger;
        }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            app.ShowHelp();
            await Task.CompletedTask;
        }

        private static string GetVersion()
            => typeof(SpotifyCmd).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
    }
}
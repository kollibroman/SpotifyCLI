namespace SpotifyClientCli.Modules.PlayerOptions
{
    [Command("play", Description = "plays an item based by name or url")]
    public class PlayOption : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        private readonly IConsole _console;
        private readonly IConfiguration _config;

        [Option("--track", Description = "Name of the track you want to play")]
        public string? Track { get; set; }
        [Option("--playlist", Description = "Paste url of playlist you want to play")]
        public string? Playlist { get; set; }
        [Option("--show", Description = "Name of the show u want to play")]
        public string? Show { get; set; }
        [Option("--episode", Description = "Name of episode you want to play")]
        public string? Episode { get; set; }
        public PlayOption(ISpotifyService service, IConsole console, IConfiguration config)
        {
            _service = service;
            _console = console;
            _config  = config;
        }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _service.UserLoggedIn(out var spotify);
            await Task.CompletedTask;
        }
    }
}
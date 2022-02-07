namespace SpotifyClientCli.Modules
{
    [Command("status", Description = "Gets current playing item and few previously played")]
    public class Status : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        private readonly AppConfig _appconfig;
        private readonly IConfiguration _config;
        private readonly IConsole _console;
        public Status(ISpotifyService service, AppConfig appConfig, IConfiguration config, IConsole console)
        {
            _service = service;
            _appconfig = appConfig;
            _config = config;
            _console = console;
        }

        public async Task OnExecuteAsync(CommandLineApplication app) //IF SOMEONE HAS BETTER WAY TO GET THIS DATA PLEASE DO IT
        {
            _service.UserLoggedIn(out var spotify);
            PlayerCurrentlyPlayingRequest request = new();
            var currentlyPlaying = await spotify!.Player.GetCurrentlyPlaying(request);
            if(currentlyPlaying.IsPlaying == true)
            {
                var item = currentlyPlaying.Item;
                if(item is FullTrack track)
                {
                    _appconfig.CurrentlyPlaying.Name = track.Name;
                    _appconfig.CurrentlyPlaying.Artists = track.Artists;
                    int duration = track.DurationMs / 1000;
                    _appconfig.CurrentlyPlaying.Duration = duration;
                    await _appconfig.SaveAsync();
                }
                else if(item is FullEpisode episode)
                {
                    _appconfig.CurrentlyPlaying.Name = episode.Name;
                    int duration = episode.DurationMs / 1000;
                    _appconfig.CurrentlyPlaying.Duration = duration;
                    await _appconfig.SaveAsync();
                }
                else if(item is FullShow show)
                {
                    _appconfig.CurrentlyPlaying.Name = show.Name;
                    await _appconfig.SaveAsync();
                }

                else
                {
                    await _console.ColoredWriteLineAsync("Nothing is currently playing", ConsoleColor.DarkGreen);
                }

                await _console.ColoredWriteLineAsync(_config["CurrentlyPlaying: Name"], ConsoleColor.Cyan);
                await _console.ColoredWriteLineAsync(_config["CurrentlyPlaying: Artists"], ConsoleColor.Cyan);
                 await _console.ColoredWriteLineAsync(_config["CurrentlyPlaying: Duration"], ConsoleColor.Cyan);

            }
        }
    }
}
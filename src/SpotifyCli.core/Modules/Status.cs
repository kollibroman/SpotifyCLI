using SpotifyCli.Db;

namespace SpotifyClientCli.Modules
{
    [Command("status", Description = "Gets current playing item and few previously played")]
    public class Status : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        private readonly SpotifyDbContext _db;
        private readonly IConfiguration _config;
        private readonly IConsole _console;
        public Status(ISpotifyService service, SpotifyDbContext db, IConfiguration config, IConsole console)
        {
            _service = service;
            _db = db;
            _config = config;
            _console = console;
        }

        public async Task OnExecuteAsync(CommandLineApplication app) //IF SOMEONE HAS BETTER WAY TO GET THIS DATA PLEASE DO IT
        {
            _service.UserLoggedIn(out var spotify);
            PlayerCurrentlyPlayingRequest request = new(PlayerCurrentlyPlayingRequest.AdditionalTypes.All);
            var currentlyPlaying = await spotify!.Player.GetCurrentlyPlaying(request);
            var playing = new SpotifyCli.Db.Entities.CurrentlyPlaying();

            if(currentlyPlaying.IsPlaying == true)
            {
                var item = currentlyPlaying.Item;
                if(item is FullTrack track)
                {
                    playing.Name = track.Name;
                    playing.Artists = track.Artists;
                    int duration = track.DurationMs / 1000;
                    playing.Duration = duration;
                }
                else if(item is FullEpisode episode)
                {
                    playing.Name = episode.Name;
                    int duration = episode.DurationMs / 1000;
                    playing.Duration = duration;
                }
                else if(item is FullShow show)
                {
                    playing.Name = show.Name;
                }

                await _db.CurrentlyPlaying.AddAsync(playing);
                await _db.SaveChangesAsync();
                await _console.ColoredWriteLineAsync(_appconfig.CurrentlyPlaying.Name, ConsoleColor.Cyan);
                await _console.ColoredWriteLineAsync(_appconfig.CurrentlyPlaying.Duration, ConsoleColor.Cyan);
                await _console.ColoredWriteLineAsync(_appconfig.CurrentlyPlaying.Artists, ConsoleColor.Cyan);
            }
            else
            {
                await _console.ColoredWriteLineAsync("Nothing is currently playing", ConsoleColor.DarkGreen);
            }
        }
    }
}
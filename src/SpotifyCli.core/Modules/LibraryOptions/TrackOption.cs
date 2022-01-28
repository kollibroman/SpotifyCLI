namespace SpotifyClientCli.Modules.LibraryOptions
{
    [Command("tracks", Description = "Shows tracks from your library")]
    public class TrackOption : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        private readonly IConsole _console;

        public TrackOption(ISpotifyService service, IConsole console)
        {
            _service = service;
            _console = console;
        }
        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _service.UserLoggedIn(out var spotify);
            var SavedTrack = await spotify!.Library.GetTracks();
            List<string> libraryItems = new();
            await foreach (var item in spotify.Paginate(SavedTrack))
            {
                libraryItems.Add(item.Track.Name);
            }
            await _console.ColoredWriteLineAsync(libraryItems.ListToString(), ConsoleColor.DarkGreen);
        }
    }
}
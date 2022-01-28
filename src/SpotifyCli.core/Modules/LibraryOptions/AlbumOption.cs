namespace SpotifyClientCli.Modules.LibraryOptions
{
    [Command("albums", Description = "Shows albums from your library")]
    public class AlbumOption : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        private readonly IConsole _console;

        public AlbumOption(ISpotifyService service, IConsole console)
        {
            _service = service;
            _console = console;
        }
        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _service.UserLoggedIn(out var spotify);
            var SavedAlbum = await spotify!.Library.GetAlbums();
            List<string> libraryItems = new();
            await foreach (var item in spotify.Paginate(SavedAlbum))
            {
                libraryItems.Add(item.Album.Name);
            }
            await _console.ColoredWriteLineAsync(libraryItems.ListToString(), ConsoleColor.DarkGreen);
        }
    }
}
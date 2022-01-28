namespace SpotifyClientCli.Modules.LibraryOptions
{
    [Command("shows", Description ="Shows shows from your library")]
    public class ShowOption : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        private readonly IConsole _console;

        public ShowOption(ISpotifyService service, IConsole console)
        {
            _service = service;
            _console = console;
        }
        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _service.UserLoggedIn(out var spotify);
            var SavedShow = await spotify!.Library.GetShows();
            List<string> libraryItems = new();
            await foreach (var item in spotify.Paginate(SavedShow))
            {
                libraryItems.Add(item.Show.Name);
            }
            await _console.ColoredWriteLineAsync(libraryItems.ListToString(), ConsoleColor.DarkGreen);
        }
    }
}
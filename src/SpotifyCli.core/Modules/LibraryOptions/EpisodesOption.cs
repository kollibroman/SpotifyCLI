namespace SpotifyClientCli.Modules.LibraryOptions
{
    [Command("episodes", Description ="Shows Episodes from your library")]
    public class EpisodesOption : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        private readonly IConsole _console;

        public EpisodesOption(ISpotifyService service, IConsole console)
        {
            _service = service;
            _console = console;
        }
        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _service.UserLoggedIn(out var spotify);
            var SavedEpisode = await spotify!.Library.GetEpisodes();
            List<string> libraryItems = new();
            await foreach (var item in spotify.Paginate(SavedEpisode))
            {
                libraryItems.Add(item.Episode.Name);
            }
            await _console.ColoredWriteLineAsync(libraryItems.ListToString(), ConsoleColor.DarkGreen);
        }
    }
}
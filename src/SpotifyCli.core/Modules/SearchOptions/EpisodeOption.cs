namespace SpotifyClientCli.Modules.SearchOptions
{
    [Command("episodes", Description = "Searches for episodes")]
    public class EpisodeOption : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        private readonly IConsole _console;
        public EpisodeOption(ISpotifyService service, IConsole console)
        {
            _service = service;
            _console = console;
        }

        [Argument(0, Description = "Type artist name you want to search for")]
        public string? EpisodeSearch { get; set; }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _service.UserLoggedIn(out var spotify);
            if (EpisodeSearch is not null)
            {
                List<string> results = new();
                var request = new SearchRequest(SearchRequest.Types.Artist, EpisodeSearch);
                var response = await spotify!.Search.Item(request);
                foreach (var item in response.Artists.Items!.Take(5))
                {
                    results.Add(item.Name);
                }
                await _console.ColoredWriteLineAsync(results.ListToString(), ConsoleColor.DarkGreen);
            }
            else
            {
                await _console.ColoredWriteLineAsync("Provide the search value", ConsoleColor.DarkRed);
            }
        }
    }
}
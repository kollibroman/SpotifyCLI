namespace SpotifyClientCli.Modules.SearchOptions
{
    [Command("show", Description = "Searches for a show")]
    public class ShowOption : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        private readonly IConsole _console;
        public ShowOption(ISpotifyService service, IConsole console)
        {
            _service = service;
            _console = console;
        }

        [Argument(0, Description = "Type artist name you want to search for")]
        public string? ShowSearch { get; set; }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _service.UserLoggedIn(out var spotify);
            if (ShowSearch is not null)
            {
                List<string> results = new();
                SearchRequest request = new(SearchRequest.Types.Artist, ShowSearch);
                var response = await spotify!.Search.Item(request);
                foreach (var item in response.Artists.Items!.Take(5))
                {
                    results.Add(item.Name);
                    results.Add(item.Uri);
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
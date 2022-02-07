namespace SpotifyClientCli.Modules.SearchOptions
{
    [Command("track", Description = "Searches for a track")]
    public class TrackOption : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        private readonly IConsole _console;
        public TrackOption(ISpotifyService service, IConsole console)
        {
            _service = service;
            _console = console;
        }

        [Argument(0, Description = "Type artist name you want to search for")]
        public string? TrackSearch { get; set; }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _service.UserLoggedIn(out var spotify);
            if (TrackSearch is not null)
            {
                List<string> results = new();
                SearchRequest request = new(SearchRequest.Types.Artist, TrackSearch);
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
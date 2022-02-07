namespace SpotifyClientCli.Modules.PlayerOptions
{
    [Command("play", Description = "adds an item to queue based by uri")]
    public class PlayOption : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        private readonly IConsole _console;
        private readonly IConfiguration _config;

        [Argument(0, Description = "Uri of the item you want to add to queue")]
        public string? SearchedItem { get; set; }
        public PlayOption(ISpotifyService service, IConsole console, IConfiguration config)
        {
            _service = service;
            _console = console;
            _config  = config;
        }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _service.UserLoggedIn(out var spotify);
            PlayerAddToQueueRequest request = new(SearchedItem!);
            await spotify!.Player.AddToQueue(request);
        }
    }
}
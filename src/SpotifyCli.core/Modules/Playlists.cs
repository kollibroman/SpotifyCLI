namespace SpotifyClientCli.Modules
{
    [Command(Description = "Gets the public playlists you have on ur account")]
    public class Playlists : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        private readonly IConsole _console;

        public Playlists(ISpotifyService service, IConsole console)
        {
            _service = service;
            _console = console;
        }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {  
            List<string> Playlists = new();
            _service.UserLoggedIn(out var spotify);

            var me = await spotify!.Playlists.CurrentUsers();
           
            await foreach (var item in spotify.Paginate(me))
            {
                Playlists.Add(item.Name.ToString());
            }
            await _console.ColoredWriteLineAsync(Playlists.ListToString(), ConsoleColor.Yellow);
        }
    }
}
using SpotifyClientCli.Modules.SearchOptions;
namespace SpotifyClientCli.Modules
{
    [Command(Description = "Searches for a specified query (Displays 5 first results)"), Subcommand(
        typeof(ArtistOption),
        typeof(PlaylistOption),
        typeof(AlbumsOption),
        typeof(EpisodeOption),
        typeof(ShowOption),
        typeof(TrackOption)

    )]
    public class Search : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        public Search(ISpotifyService service)
        {
            _service = service;
        }
        public Task OnExecuteAsync(CommandLineApplication app)
        {
           app.ShowHelp();
           return Task.CompletedTask;
           
        }
    }
}
using SpotifyClientCli.Modules.SearchOptions;
namespace SpotifyClientCli.Modules
{
    [Command(Description = "Searches for a track or playlist (Displays 5 of them)"), Subcommand(
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
        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _service.UserLoggedIn(out var spotify);

           app.ShowHelp();
           await Task.CompletedTask;
           
        }
    }
}
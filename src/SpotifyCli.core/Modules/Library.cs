using SpotifyClientCli.Modules.LibraryOptions;
namespace SpotifyClientCli.Modules
{
    [Command(Description = "Shows ur library items"), Subcommand(
       typeof(AlbumOption),
       typeof(ShowOption),
       typeof(EpisodesOption),
       typeof(TrackOption)
    )]
    public class Library : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        public Library(ISpotifyService service)
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
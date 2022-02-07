using SpotifyClientCli.Modules.PlayerOptions;
namespace SpotifyClientCli.Modules
{
    [Command(Description = "Plays a track"), Subcommand(
        typeof(PlayOption),
        typeof(DeviceOption),
        typeof(PauseOption),
        typeof(NextOption),
        typeof(ResumeOption),
        typeof(VolumeOption)
    )]
    public class Player : ISpotifyBase
    {
        private readonly ISpotifyService _service;
    
        public Player(ISpotifyService service)
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
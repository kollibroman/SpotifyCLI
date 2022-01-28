using SpotifyClientCli.Modules.PlayerOptions;
namespace SpotifyClientCli.Modules
{
    [Command(Description = "Plays a track"), Subcommand(
        typeof(PlayOption),
        typeof(DeviceOption),
        typeof(PauseOption),
        typeof(NextOption),
        typeof(QueueOption),
        typeof(ResumeOption)
    )]
    public class Player : ISpotifyBase
    {
        private readonly ISpotifyService _service;
    
        public Player(ISpotifyService service)
        {
            _service = service;
        }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {   
            app.ShowHelp();
            await Task.CompletedTask;
        }
    }
}
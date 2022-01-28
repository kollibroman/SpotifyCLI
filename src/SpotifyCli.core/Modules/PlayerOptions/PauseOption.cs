namespace SpotifyClientCli.Modules.PlayerOptions
{
    [Command("pause", Description = "Pauses current track")]
    public class PauseOption : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        public PauseOption(ISpotifyService service)
        {
            _service = service;
        }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            await Task.CompletedTask;
        }
    }
}
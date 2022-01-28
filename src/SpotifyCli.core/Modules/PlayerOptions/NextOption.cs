namespace SpotifyClientCli.Modules.PlayerOptions
{
    [Command("next", Description = "plays next track")]
    public class NextOption : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        public NextOption(ISpotifyService service)
        {
            _service = service;
        }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            await Task.CompletedTask;
        }
    }
}
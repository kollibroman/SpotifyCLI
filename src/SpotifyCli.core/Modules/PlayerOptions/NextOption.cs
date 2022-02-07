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
            _service.UserLoggedIn(out var spotify);
            var player = spotify!.Player;
            await player.SkipNext();
        }
    }
}
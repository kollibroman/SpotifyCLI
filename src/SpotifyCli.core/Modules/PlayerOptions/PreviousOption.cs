namespace SpotifyCli.core.Modules.PlayerOptions
{
    [Command("prev", Description = "Goes back to the previous track")]
    class PreviousOption : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        public PreviousOption(ISpotifyService service)
        {
            _service = service;
        }
        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _service.UserLoggedIn(out var spotify);
            await spotify!.Player.SkipPrevious();
        }
    }
}
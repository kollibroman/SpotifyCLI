namespace SpotifyClientCli.Modules.PlayerOptions
{
    [Command("queue", Description = "Shows current queue")]
    public class QueueOption : ISpotifyBase
    {
        private readonly ISpotifyService _service;

        public QueueOption(ISpotifyService service)
        {
            _service = service;
        }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _service.UserLoggedIn(out var spotify);

            await Task.CompletedTask;
        }
    }
}
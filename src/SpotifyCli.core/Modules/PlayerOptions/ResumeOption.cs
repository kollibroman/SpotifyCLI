namespace SpotifyClientCli.Modules.PlayerOptions
{
    [Command("resume", Description = "resumes paused track")]
    public class ResumeOption : ISpotifyBase
    {
        private readonly ISpotifyService _service ;
        public ResumeOption(ISpotifyService service)
        {
            _service = service;
        }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            await Task.CompletedTask;
        }
    }
}
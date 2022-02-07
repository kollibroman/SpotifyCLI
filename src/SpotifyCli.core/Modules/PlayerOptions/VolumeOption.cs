namespace SpotifyClientCli.Modules.PlayerOptions
{
    [Command("set-volume", Description ="Set volume to intenger value")]
    public class VolumeOption : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        public VolumeOption(ISpotifyService service)
        {
            _service = service;
        }
        [Argument(0)]
        public int VolumeValue { get; set; }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _service.UserLoggedIn(out var spotify);
            var request = new PlayerVolumeRequest(VolumeValue);
            await spotify!.Player.SetVolume(request);
        }
    }
}
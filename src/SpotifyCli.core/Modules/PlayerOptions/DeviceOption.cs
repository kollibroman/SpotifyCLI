namespace SpotifyClientCli.Modules.PlayerOptions
{
    [Command("devices", Description = "Selects device in which u want to play")]
    public class DeviceOption : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        private readonly IConsole _console;
        private readonly AppConfig _config;
        public DeviceOption(ISpotifyService service, IConsole console, AppConfig config)
        {
            _service = service;
            _console = console;
            _config = config;
        }
        [Option("--device-name", Description = "Set device name")]
        public string? DeviceName { get; set; }

        [Option("--device-id", Description = "Selects device on which to play by id")]
        public string? DeviceId { get; set; }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _service.UserLoggedIn(out var spotify);
            List<string> result = new();
            var device = await spotify!.Player.GetAvailableDevices();
            foreach(var item in device.Devices)
            {
                result.Add(item.Name);
                result.Add(item.Id);
            }
            await _console.ColoredWriteLineAsync(result.ListToString(), ConsoleColor.DarkYellow);

            if(DeviceName is not null && DeviceId is not null)
            {
                _config.Device.Name = DeviceName;
                _config.Device.ID = DeviceId;
                await _config.SaveAsync();
            }
        }
    }
}
using SpotifyCli.Db;
using SpotifyCli.Db.Entities;

namespace SpotifyClientCli.Modules.PlayerOptions
{
    [Command("devices", Description = "Selects device in which u want to play")]
    public class DeviceOption : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        private readonly IConsole _console;
        private readonly SpotifyDbContext _db;
        public DeviceOption(ISpotifyService service, IConsole console, SpotifyDbContext db)
        {
            _service = service;
            _console = console;
            _db = db;
        }
        [Option("--device-name", Description = "Set device name")]
        public string? DeviceName { get; }

        [Option("--device-id", Description = "Selects device on which to play by id")]
        public string? DeviceId { get; }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _service.UserLoggedIn(out var spotify);
            List<string> result = new();
            var devices = await spotify!.Player.GetAvailableDevices();
            foreach(var item in devices.Devices)
            {
                result.Add(item.Name);
                result.Add(item.Id);
            }
            await _console.ColoredWriteLineAsync(result.ListToString(), ConsoleColor.DarkYellow);

            if(DeviceName is not null && DeviceId is not null)
            {
                var device = new SpotifyCli.Db.Entities.Device()
                {
                    Id = 1,
                    DeviceId = DeviceName,
                    Name = DeviceName
                };
                await _db.SaveChangesAsync();
            }
        }
    }
}
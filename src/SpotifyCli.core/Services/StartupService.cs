namespace SpotifyClientCli.Services
{
    public class StartupService : IHostedService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<StartupService> _logger;

        public StartupService(IConfiguration config, ILogger<StartupService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken token)
        {
            await Load();
        }

        public async Task StopAsync(CancellationToken token)
        {
            await Task.CompletedTask;
        }

        public static async Task<AppConfig> Load()
        {
            var config = new AppConfig();
            Directory.CreateDirectory(Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData,
                Environment.SpecialFolderOption.Create
        ),
        "SpotifyCli"
        ));            

            if (!File.Exists(config.AppConfigFilePath))
            {
               await config.SaveAsync();
               return config;
            }
       
            var configContent = await File.ReadAllTextAsync(config.AppConfigFilePath);
        
            return JsonConvert.DeserializeObject<AppConfig>(configContent)!;
        }
    }
}
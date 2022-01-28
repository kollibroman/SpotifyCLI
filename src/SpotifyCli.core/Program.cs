using Serilog;
namespace SpotifyCientCli
{
   static class Program 
    {
        static async Task<int> Main(string[] args)
        {   
            var config = new AppConfig();
            var builder = new ConfigurationBuilder()
                .AddJsonFile(config.AppConfigFilePath, true)
                .AddEnvironmentVariables()
                .Build();

            var host = new HostBuilder()
            .ConfigureAppConfiguration(x =>
            {
                x.AddConfiguration(builder);
            })
            .ConfigureServices((config, services) =>
            {
                services.AddHostedService<StartupService>();
                services.AddHttpClient();
                services.AddSingleton<IConsole>(PhysicalConsole.Singleton);
                services.AddSingleton(config);
                services.AddSingleton<AppConfig>();
                services.AddSingleton<ISpotifyService, SpotifyService>();
            })
            .ConfigureLogging(x =>
            {
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder)
                    .WriteTo.Console()
                    .Enrich.FromLogContext()
                    .CreateLogger();
            })
            .UseConsoleLifetime()
            .UseSerilog();

            try
            {
                return await host.RunCommandLineApplicationAsync<SpotifyCmd>(args);
            }
            catch (Exception ex)
            {
                WriteLine(ex.InnerException);
                return 1;
            }
        }
    }
}
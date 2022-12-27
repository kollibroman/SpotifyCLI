using Serilog;
using SpotifyCli.Db;

namespace SpotifyCientCli
{
   static class Program 
    {
        static async Task<int> Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureServices((config, services) =>
                {
                    services.AddHttpClient();
                    services.AddSingleton<IConsole>(PhysicalConsole.Singleton);
                    services.AddSingleton(config);
                    services.AddDbContext<SpotifyDbContext>();
                    services.AddSingleton<ISpotifyService, SpotifyService>();
                })
                .ConfigureLogging(x =>
                {
                    Log.Logger = new LoggerConfiguration()
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
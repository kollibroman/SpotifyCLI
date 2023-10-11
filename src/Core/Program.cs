using Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Core.Commands;
using Core.Interfaces;
using Core.Services;
//using Core.Tests;

namespace Core;

public class Program
{
    public static async Task Main(string[] args)
    {
        var config = new AppConfig();
        var services = new ServiceCollection()
            .AddSingleton<IDataHandler>()
            .AddSingleton(config)
            .AddSingleton<AppConfig>()
            .AddScoped<ICredAdder, CredAdder>()
            .AddScoped<ILoginService, LoginService>();

        var registrer = new TypeRegister(services);

        var app = new CommandApp(registrer);

        app.Configure(configuration => 
        {
            configuration.SetApplicationName("spot-cli");
            configuration.AddCommand<AddCommand>("add-cred")
                .WithDescription("Adds your client credentials from dev app");
            configuration.AddCommand<LoginCommand>("login")
                .WithDescription("logs you into your accoount");
        });

        var startup = new StartupService();

        await startup.Load();

        await app.RunAsync(args);
    }
}
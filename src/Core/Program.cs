using Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Core.Commands;
using Core.Database;
using Core.Helpers;
using Core.Interfaces;
using Core.Services;
using Spectre.Console;

//using Core.Tests;

namespace Core;

public class Program
{
    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection()
            .AddDbContext<SpotDbContext>()
            .AddSingleton<DataHandler>()
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
        
        var serviceProvider = services.BuildServiceProvider();
        var startupService = serviceProvider.GetRequiredService<StartupService>();
        
        await startupService.LoadDatabaseDataASync();

        try
        {
            await app.RunAsync(args);
        }
        catch (System.Exception e)
        {
            AnsiConsole.WriteException(e);
            throw;
        }
    }
}
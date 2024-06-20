using Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Core.Commands;
using Core.Helpers;
using Core.Interfaces;
using Core.Services;
using Database;
using Spectre.Console;

//using Core.Tests;

namespace Core;

public class Program
{
    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection()
            .AddSqlite<SpotDbContext>("Data Source=spot.db;")
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
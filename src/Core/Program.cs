using Database;
using Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Core.Commands;
using Core.Interfaces;
using Core.Services;
using Core.Settings;

namespace Core;

public class Program
{
    public static int Main(string[] args)
    {
        var services = new ServiceCollection()
            .AddDbContext<SpotDbContext>()
            .AddScoped<ICredAdder, CredAdder>();

        var registrer = new TypeRegister(services);


        var app = new CommandApp<AddCommand>(registrer);
        return app.Run(args);
    }
}
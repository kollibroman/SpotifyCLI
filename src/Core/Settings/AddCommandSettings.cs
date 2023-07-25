using System;
using System.ComponentModel;
using Database;
using Spectre.Console.Cli;

namespace Core.Settings
{
    public class AddCommandSettings : CommandSettings
    {
        [CommandOption("--clientid <ID>")]
        [Description("Your spotify apps clientId")]
        public string? ClientId { get; set; }

        [CommandOption("--clientsecret <secret>")]
        [Description("Your spotify apps clientSecret")]
        public string? ClientSecret { get; set; }
    }
}

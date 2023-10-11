using System;
using Core.Interfaces;
using Core.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Core.Commands
{
    public class AddCommand : AsyncCommand<AddCommandSettings>
    {
       private readonly ICredAdder _adder;

       public AddCommand(ICredAdder adder)
       {
            _adder = adder;
       }

       public override async Task<int> ExecuteAsync(CommandContext context, AddCommandSettings settings)
       {
            var prompt1 = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your client Id:")
                .PromptStyle("green") 
            );

            var prompt2 = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your client Secret:")
                .PromptStyle("green") 
            );

            await _adder.AddCredentials(prompt1, prompt2);

            return 0;
       }
    }
}

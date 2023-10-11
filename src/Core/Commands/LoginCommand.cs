using System.Threading.Tasks;
using Core.Interfaces;
using Spectre.Console.Cli;

namespace Core.Commands
{
    public class LoginCommand : AsyncCommand
    {
        private readonly ILoginService _service;

        public LoginCommand(ILoginService service)
        {
            _service = service;
        }

        public override async Task<int> ExecuteAsync(CommandContext commandContext)
        {
            await _service.Login();

            return 1;
        }
    }
}

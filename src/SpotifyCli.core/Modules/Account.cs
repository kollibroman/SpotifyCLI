using System.Linq;

namespace SpotifyClientCli.Modules
{
    [Command(Description = "Shows ur Account")]
    public class Account : ISpotifyBase
    {
        private readonly ISpotifyService _service;
        private readonly IConsole _console;
        public Account(ISpotifyService service, IConsole console)
        {
            _service = service;
            _console =  console;
        }
        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _service.UserLoggedIn(out var spotify);
            var me = await spotify!.UserProfile.Current();

            List<string> AccountContents = new();  //For this moment idk how to don't hardcode this :D
            AccountContents.Add(me.DisplayName);
            AccountContents.Add(me.Country);
            AccountContents.Add(me.Email);
            AccountContents.Add(Convert.ToString(me.Followers.Total));
            AccountContents.Add(me.Uri);
            AccountContents.Add(me.Type);
            AccountContents.Add(me.Id);
            AccountContents.Add(me.Href);
            AccountContents.Add(me.Product);
            
           await _console.ColoredWriteLineAsync(AccountContents.ListToString(), ConsoleColor.Magenta);
        }
    }
}
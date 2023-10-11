using Core.Interfaces;
using Core.Data;
using Spectre.Console;

namespace Core.Services
{
    public class CredAdder : ICredAdder
    {
        private readonly AppConfig _config;
        public CredAdder(AppConfig config)
        {
            _config = config;
        }

        public async Task AddCredentials(string ClientId, string ClientSecret)
        {
            _config.Data.ClientId = ClientId;
            _config.Data.ClientSecret = ClientSecret;

            await _config.SaveAsync();
            
            AnsiConsole.WriteLine("Data succesfully added");
        }
    }
}

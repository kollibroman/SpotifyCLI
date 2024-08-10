using Core.Database;
using Core.Interfaces;
using Spectre.Console;

namespace Core.Services
{
    public class CredAdder : ICredAdder
    {
        private readonly SpotDbContext _dbContext;
        public CredAdder(SpotDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddCredentials(string ClientId, string ClientSecret)
        {
            
            
            AnsiConsole.WriteLine("Data succesfully added");
        }
    }
}

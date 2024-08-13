using Core.Database;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task AddCredentials(string clientId, string clientSecret)
        {
            await _dbContext.ClientData.AddAsync(new()
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            });

            await _dbContext.SaveChangesAsync();
            
            AnsiConsole.WriteLine("Data succesfully added");
            
            var creds = await _dbContext.ClientData.SingleOrDefaultAsync(x => x.Id == 1) ?? default!;
            
            AnsiConsole.WriteLine(creds.ClientId + " " + creds.ClientSecret);
        }
    }
}

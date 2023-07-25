using System;
using Core.Interfaces;
using Database;
using Database.Entities;
using Spectre.Console;

namespace Core.Services
{
    public class CredAdder : ICredAdder
    {
        private readonly SpotDbContext _db;

        public CredAdder(SpotDbContext dbContext)
        {
            _db = dbContext;
        }

        public void AddCredentials(string ClientId, string ClientSecret)
        {
            var AppDetails = new AppDetails()
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret 
            };

            _db.AppDetails.Add(AppDetails);
            _db.SaveChanges();

            _db.SaveChangesFailed += SaveFailed!;
        }

        static void SaveFailed(object sender, EventArgs eventArgs)
        {
            AnsiConsole.WriteLine("Saving changes failed");
        }
    }
}

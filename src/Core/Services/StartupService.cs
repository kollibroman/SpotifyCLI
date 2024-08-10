using Core.Database;
using Core.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Services
{
    public class StartupService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly DataHandler _dataHandler;
        
        public StartupService(IServiceScopeFactory serviceScopeFactory, DataHandler dataHandler)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _dataHandler = dataHandler;
        }
        
        public async Task LoadDatabaseDataASync()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotDbContext>();
            
            _dataHandler.ClientData = await dbContext.ClientData.SingleOrDefaultAsync(x => x.Id == 1) ?? default!;
            _dataHandler.Device = await dbContext.Device.SingleOrDefaultAsync(x => x.Id == 1) ?? default!;
            _dataHandler.Token = await dbContext.Token.SingleOrDefaultAsync(x => x.Id == 1) ?? default!;
            _dataHandler.Account = await dbContext.UsrAccount.SingleOrDefaultAsync(x => x.Id == 1) ?? default!;
        }
    }
}

using Core.Database;
using Core.Database.Models;
using Core.Helpers;
using SpotifyAPI.Web.Auth;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using SpotifyAPI.Web;
using Spectre.Console;

namespace Core.Services
{
    public class LoginService : ILoginService
    {
        private static EmbedIOAuthServer? _server;
        private readonly SemaphoreSlim _signal = new(0,1);
        private readonly DataHandler _handler;
        private readonly SpotDbContext _dbContext;
        private readonly IStartupService _startupService;

        public LoginService(DataHandler handler, SpotDbContext dbContext, IStartupService startupService)
        {
            _handler = handler;
            _dbContext = dbContext;
            _startupService = startupService;
        }

        public async Task Login()
        {
            await _startupService.LoadDatabaseDataASync();
            
            _server = new EmbedIOAuthServer(new Uri("http://localhost:5000/callback"), 5000);
            await _server.Start();

            _server.AuthorizationCodeReceived += OnAuthCodeReceived;
            _server.ErrorReceived += Error;

            var request = new LoginRequest(_server.BaseUri,
                _handler.ClientData.ClientId! ,LoginRequest.ResponseType.Code)
            {
                Scope = new List<string> {  Scopes.AppRemoteControl,
                    Scopes.PlaylistModifyPrivate,
                    Scopes.PlaylistModifyPublic,
                    Scopes.PlaylistReadCollaborative,
                    Scopes.PlaylistReadPrivate,
                    Scopes.Streaming,
                    Scopes.UgcImageUpload,
                    Scopes.UserFollowModify,
                    Scopes.UserFollowRead,
                    Scopes.UserLibraryModify,
                    Scopes.UserLibraryRead,
                    Scopes.UserModifyPlaybackState,
                    Scopes.UserReadCurrentlyPlaying,
                    Scopes.UserReadEmail,
                    Scopes.UserReadPlaybackPosition,
                    Scopes.UserReadPlaybackState,
                    Scopes.UserReadPrivate,
                    Scopes.UserReadRecentlyPlayed,
                    Scopes.UserTopRead }
            };
            BrowserUtil.Open(request.ToUri());

            await _signal.WaitAsync();
        }

        private async Task OnAuthCodeReceived(object sender, AuthorizationCodeResponse response)
        {
            await _server!.Stop();

            var config = SpotifyClientConfig.CreateDefault();

            var tokenResponse = await new OAuthClient(config).RequestToken(
                new AuthorizationCodeTokenRequest(
              _handler.ClientData.ClientId!, _handler.ClientData.ClientSecret!, response.Code, new Uri("http://localhost:5000/callback")));
            
            var spotify = new SpotifyClient(tokenResponse.AccessToken);
            
            var acc = await spotify.UserProfile.Current();  
            AnsiConsole.WriteLine($"Hello {acc.DisplayName}");
            
            var account = new UsrAccount
            {
                DisplayName = acc.DisplayName,
                Uri = acc.Uri,
                UserId = acc.Id
            };

            var token = new Token
            {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                ExpiresIn = tokenResponse.ExpiresIn,
                TokenType = tokenResponse.TokenType,
                CreatedAt = DateTime.Now
            };
            
            await SaveDataAsync(token, account);
            
            _signal.Release();    
        }

        private async Task Error(object sender, string error, string? state)
        {
            Console.WriteLine($"Aborting authorization, error received: {error}");
            await _server!.Stop();

            _signal.Release();
        }

        public async Task Logout()
        {
            var token = await _dbContext.Token.SingleOrDefaultAsync(x => x.Id == 1);
            var account = await _dbContext.UsrAccount.SingleOrDefaultAsync(x => x.Id == 1);

            if (token is not null && account is not null)
            {
                _dbContext.Token.Remove(token);
                _dbContext.UsrAccount.Remove(account);
            }
        }
        
        private async Task SaveDataAsync(Token token, UsrAccount account)
        {
            _dbContext.Token.Update(token);
            _dbContext.UsrAccount.Update(account);
            
            await _dbContext.SaveChangesAsync();
        }
    }
}

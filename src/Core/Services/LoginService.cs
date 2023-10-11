using System.Runtime.Versioning;
using SpotifyAPI.Web.Auth;
using Core.Interfaces;
using SpotifyAPI.Web;
using Spectre.Console;

namespace Core.Services
{
    public class LoginService : ILoginService
    {
        private static EmbedIOAuthServer _server;
        private readonly SemaphoreSlim signal = new(0,1);
        private readonly DataHandler _handler;
        private readonly AppConfig _config;

        public LoginService(DataHandler handler, AppConfig config)
        {
           _handler = handler;
           _config = config;
        }

        public async Task Login()
        {
            _server = new EmbedIOAuthServer(new Uri("http://localhost:5000/callback"), 5000);
            await _server.Start();

            _server.AuthorizationCodeReceived += OnAuthCodeReceived;
            _server.ErrorReceived += Error;

            var request = new LoginRequest(_server.BaseUri,
    _handler.data.ClientId ,LoginRequest.ResponseType.Code)
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

            await signal.WaitAsync();
        }

        private async Task OnAuthCodeReceived(object sender, AuthorizationCodeResponse response)
        {
            await _server.Stop();

            var config = SpotifyClientConfig.CreateDefault();

            var TokenResponse = await new OAuthClient(config).RequestToken(
                new AuthorizationCodeTokenRequest(
              _handler.data.ClientId, _handler.data.ClientSecret, response.Code, new Uri("http://localhost:5000/callback")));

            var spotify = new SpotifyClient(TokenResponse.AccessToken);

            _config.Token.AccessToken = TokenResponse.AccessToken;
            _config.Token.RefreshToken = TokenResponse.RefreshToken;
            _config.Token.TokenType = TokenResponse.TokenType;
            _config.Token.CreatedAt = TokenResponse.CreatedAt;
            _config.Token.ExpiresIn = TokenResponse.ExpiresIn;
            
            var acc = await spotify.UserProfile.Current();  
            AnsiConsole.WriteLine($"Hello {acc.DisplayName}");

            _config.Account.DisplayName = acc.DisplayName;
            _config.Account.Uri = acc.Uri;
            _config.Account.UserId = acc.Id;
            await _config.SaveAsync();
            signal.Release();    
        }

        private async Task Error(object sender, string error, string state)
        {
            Console.WriteLine($"Aborting authorization, error received: {error}");
            await _server.Stop();

            signal.Release();
        }

        public Task Logout()
        {
            throw new  NotImplementedException();
        }
    }
}

using Core.Helpers;
using SpotifyAPI.Web.Auth;
using Core.Interfaces;
using SpotifyAPI.Web;
using Spectre.Console;

namespace Core.Services
{
    public class LoginService : ILoginService
    {
        private static EmbedIOAuthServer? _server;
        private readonly SemaphoreSlim _signal = new(0,1);
        private readonly DataHandler _handler;

        public LoginService(DataHandler handler)
        {
           _handler = handler;
        }

        public async Task Login()
        {
            _server = new EmbedIOAuthServer(new Uri("http://localhost:5000/callback"), 5000);
            await _server.Start();

            _server.AuthorizationCodeReceived += OnAuthCodeReceived;
            _server.ErrorReceived += Error;

            var request = new LoginRequest(_server.BaseUri,
                _handler.ClientData.ClientId ,LoginRequest.ResponseType.Code)
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
              _handler.ClientData.ClientId, _handler.ClientData.ClientSecret, response.Code, new Uri("http://localhost:5000/callback")));

            var spotify = new SpotifyClient(tokenResponse.AccessToken);

            _handler.Token.AccessToken = tokenResponse.AccessToken;
            _handler.Token.RefreshToken = tokenResponse.RefreshToken;
            _handler.Token.TokenType = tokenResponse.TokenType;
            _handler.Token.CreatedAt = tokenResponse.CreatedAt;
            _handler.Token.ExpiresIn = tokenResponse.ExpiresIn;
            
            var acc = await spotify.UserProfile.Current();  
            AnsiConsole.WriteLine($"Hello {acc.DisplayName}");

            _handler.Account.DisplayName = acc.DisplayName;
            _handler.Account.Uri = acc.Uri;
            _handler.Account.UserId = acc.Id;
            _signal.Release();    
        }

        private async Task Error(object sender, string error, string? state)
        {
            Console.WriteLine($"Aborting authorization, error received: {error}");
            await _server!.Stop();

            _signal.Release();
        }

        public Task Logout()
        {
            throw new  NotImplementedException();
        }
    }
}

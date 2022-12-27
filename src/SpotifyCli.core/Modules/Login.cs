using SpotifyCli.Db;
using SpotifyCli.Db.Entities;

namespace SpotifyClientCli.Modules
{
    [Command(Description = "starts auth process")]
    [VersionOptionFromMember("-v", MemberName = nameof(GetVersion))]
    public class Login : ISpotifyBase
    {
        private static EmbedIOAuthServer? _server;
        private readonly SpotifyDbContext _db;
        private readonly ISpotifyService _service;
        private readonly SemaphoreSlim signal = new(0, 1);
        private readonly ILogger<Login> _logger;
        private readonly IConsole _console;

        public Login(ILogger<Login> logger, ISpotifyService service, SpotifyDbContext db, IConsole console)
        {
            _logger = logger;
            _db = db;
            _service = service;
            _console = console;
        }
        
        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _server = new EmbedIOAuthServer(new Uri("http://localhost:5000/callback"), 5000);

            await _server.Start();

            _server.AuthorizationCodeReceived += OnAuthCodeReceived;
            _server.ErrorReceived += OnErrorReceived;
            
            var OtherApp = _db.AppDetails.SingleOrDefault(i => i.Id == 1);

            LoginRequest request = new(_server.BaseUri, OtherApp.ClientId!, LoginRequest.ResponseType.Code)
            {
                Scope = new List<string> {
                    Scopes.AppRemoteControl,
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
                    Scopes.UserTopRead,
                    }
            };
            
            BrowserUtil.Open(request.ToUri());

            await signal.WaitAsync();
        }

        private async Task OnAuthCodeReceived(object sender, AuthorizationCodeResponse response)
        {
            await _server.Stop();
            var app = _db.AppDetails.SingleOrDefault(i => i.Id == 1);
            var config = SpotifyClientConfig.CreateDefault();
            var TokenResponse = await new OAuthClient(config).RequestToken(
                new AuthorizationCodeTokenRequest(
                    app.ClientId, app.ClientSecret, response.Code, new Uri("http://localhost:5000/callback")
                )
            );
            SpotifyClient spotify = new(TokenResponse.AccessToken);

            var Token = new Token()
            {
                Id = 1,
                AccessToken = TokenResponse.AccessToken,
                RefreshToken = TokenResponse.RefreshToken,
                ExpiresIn = TokenResponse.ExpiresIn,
                TokenType = TokenResponse.TokenType,
                CreatedAt = TokenResponse.CreatedAt
            };
            await _db.Tokens.AddAsync(Token);

            var me = await spotify.UserProfile.Current();
            var account = new UsrAccount()
            {
                Id = 1,
                DisplayName = me.DisplayName,
                Uri = me.Uri
            };
            await _db.UsrAccount.AddAsync(account);
                _logger.LogInformation($"Hello {me.DisplayName}");
            await _db.SaveChangesAsync();
            signal.Release();           
        }
  
        private  async Task OnErrorReceived(object sender, string error, string state)
        {  
            await _console.ColoredWriteLineAsync($"Aborting authorization, error received: {error}", ConsoleColor.DarkRed);
            await _server!.Stop();

            signal.Release();
        }

           private static string GetVersion() =>
            typeof(Login).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
    }
}
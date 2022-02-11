namespace SpotifyClientCli.Modules
{
    [Command(Description = "starts auth process")]
    [VersionOptionFromMember("-v", MemberName = nameof(GetVersion))]
    public class Login : ISpotifyBase
    {
        private static EmbedIOAuthServer? _server;
        private readonly AppConfig _appconfig;
        private readonly ISpotifyService _service;
        private readonly SemaphoreSlim signal = new(0, 1);
        private readonly ILogger<Login> _logger;
        private readonly IConsole _console;

        public Login(ILogger<Login> logger, ISpotifyService service, AppConfig appConfig, IConsole console)
        {
            _logger = logger;
            _appconfig = appConfig;
            _service = service;
            _console = console;
        }
        
        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            _server = new EmbedIOAuthServer(new Uri("http://localhost:5000/callback"), 5000);

            await _server.Start();

            _server.AuthorizationCodeReceived += OnAuthCodeReceived;
            _server.ErrorReceived += OnErrorReceived;

            LoginRequest request = new(_server.BaseUri, _appconfig.App.ClientId!, LoginRequest.ResponseType.Code)
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
            var config = SpotifyClientConfig.CreateDefault();
            var TokenResponse = await new OAuthClient(config).RequestToken(
                new AuthorizationCodeTokenRequest(
                    _appconfig.App.ClientId!, _appconfig.App.ClientSecret!, response.Code, new Uri("http://localhost:5000/callback")
                )
            );
            SpotifyClient spotify = new(TokenResponse.AccessToken);
                _appconfig.Token.AccessToken = TokenResponse.AccessToken;
                _appconfig.Token.RefreshToken = TokenResponse.RefreshToken;
                _appconfig.Token.CreatedAt = TokenResponse.CreatedAt;
                _appconfig.Token.ExpiresIn = TokenResponse.ExpiresIn;
                _appconfig.Token.TokenType = TokenResponse.TokenType;

            var me = await spotify.UserProfile.Current();
            _appconfig.Account.DisplayName = me.DisplayName;
            _appconfig.Account.Id = me.Id;
            _appconfig.Account.Uri = me.Uri;
            await _appconfig.SaveAsync();
            _logger.LogInformation($"Hello {me.DisplayName}");
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
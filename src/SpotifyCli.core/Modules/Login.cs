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

            var (verifier, challenge) = PKCEUtil.GenerateCodes(120);
            await _server.Start();

            _server.AuthorizationCodeReceived += async (sender, response) =>
            {
                await _server!.Stop();

               PKCETokenResponse tokenResponse = await _service.OAuth.RequestToken(
                   new PKCETokenRequest(_appconfig.App.ClientId!, response.Code, _server.BaseUri, verifier)
               );


                _appconfig.Token.AccessToken = tokenResponse.AccessToken;
                _appconfig.Token.RefreshToken = tokenResponse.RefreshToken;
                _appconfig.Token.CreatedAt = tokenResponse.CreatedAt;
                _appconfig.Token.ExpiresIn = tokenResponse.ExpiresIn;
                _appconfig.Token.TokenType = tokenResponse.TokenType;
                
                var config = SpotifyClientConfig.CreateDefault(tokenResponse.AccessToken).WithHTTPLogger(new SimpleConsoleHTTPLogger());
                var spotify = new SpotifyClient(config);
                var me = await spotify.UserProfile.Current();

                _appconfig.Account.Id = me.Id;
                _appconfig.Account.DisplayName = me.DisplayName;
                _appconfig.Account.Uri = me.Uri;

                await _appconfig.SaveAsync();
                
                _logger.LogInformation($"Hello {me.DisplayName}");

                signal.Release();
            };
            _server.ErrorReceived += OnErrorReceived!;

            var request = new LoginRequest(_server.BaseUri, _appconfig.App.ClientId!, LoginRequest.ResponseType.Code)
            {
                CodeChallenge = challenge,
                CodeChallengeMethod = "S256",
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
namespace SpotifyClientCli.Services.Interfaces
{
    public class SpotifyService : ISpotifyService
    {
        private ILogger<SpotifyService> _logger;
        private readonly AppConfig _appconfig;
        private SpotifyClientConfig _config;
        private SpotifyClient? _spotify;
        private OAuthClient _oauth;
        private readonly IConfiguration _conf;
        public SpotifyService(ILogger<SpotifyService> logger, AppConfig appConfig, IConfiguration config)
        {
            _logger = logger;
            _appconfig = appConfig;
            _conf = config;

            if (!string.IsNullOrEmpty(_conf.GetValue<string>("Token:RefreshToken")))
            {
                _config = CreateForUser();
                _spotify = new SpotifyClient(_config);
            }

            else
            {
                _config = SpotifyClientConfig.CreateDefault();
            }

            _oauth = new OAuthClient(_config);
        }

        public SpotifyClientConfig CreateForUser()
        {
            return SpotifyClientConfig
                .CreateDefault()
                .WithAuthenticator(new PKCEAuthenticator(
                    _appconfig.App.ClientId!, new PKCETokenResponse
                    {
                        AccessToken = _conf.GetValue<string>("Token:AccessToken"),
                        CreatedAt = _conf.GetValue<DateTime>("Token:CreatedAt"),
                        ExpiresIn = _conf.GetValue<int>("Token:ExpiresIn"),
                        TokenType = _conf.GetValue<string>("Token:TokenType")
                    }
                ))
                .WithRetryHandler(new SimpleRetryHandler());
        }


        public bool UserLoggedIn([NotNull]out SpotifyClient? spotify)
        {
            if (Spotify == null || !(Config.Authenticator is PKCEAuthenticator))
            {
                spotify = null;
                _logger.LogInformation("Ur not logged in");
                Environment.Exit(1);        
            }

            spotify = Spotify;
            return true;
        }

        public SpotifyClientConfig Config { get => _config; }
        public SpotifyClient? Spotify { get => _spotify; }
        public OAuthClient OAuth { get => _oauth; }

    }
}
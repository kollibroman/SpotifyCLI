using SpotifyCli.Db;

namespace SpotifyClientCli.Services
{
    public class SpotifyService : ISpotifyService
    {
        private ILogger<SpotifyService> _logger;
        private readonly SpotifyDbContext _db;
        private SpotifyClientConfig _config;
        private SpotifyClient? _spotify;
        private OAuthClient _oauth;
        private readonly IConfiguration _conf;
        public SpotifyService(ILogger<SpotifyService> logger, SpotifyDbContext db, IConfiguration config)
        {
            _logger = logger;
            _db = db;
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
            var token = _db.Tokens.SingleOrDefault(i => i.Id == 1);
            return SpotifyClientConfig
                .CreateDefault()
                .WithAuthenticator(new PKCEAuthenticator(
                    _db.AppDetails.SingleOrDefault(i => i.Id == 1).ClientId!, new PKCETokenResponse
                    {
                        AccessToken = token.AccessToken,
                        CreatedAt = token.CreatedAt,
                        ExpiresIn = token.ExpiresIn,
                        TokenType = token.TokenType
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
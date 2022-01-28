namespace SpotifyClientCli.Services
{
    public interface ISpotifyService
    {
        bool UserLoggedIn(out SpotifyClient? spotify);
        SpotifyClientConfig Config { get; }

        SpotifyClient? Spotify { get; }

        OAuthClient OAuth { get; }
    }
}
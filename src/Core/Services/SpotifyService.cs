using System.Diagnostics.CodeAnalysis;
using Core.Exception;
using Core.Helpers;
using Core.Interfaces;
using SpotifyAPI.Web;

namespace Core.Services;

public class SpotifyService
{
    private readonly DataHandler _handler;
    public SpotifyService(DataHandler handler)
    {
        _handler = handler;

        if (!string.IsNullOrEmpty(_handler.Token.RefreshToken))
        {
            Config = CreateForUser();
            Spotify = new SpotifyClient(Config);
        }

        else
        {
            Config = SpotifyClientConfig.CreateDefault();
        }

        OAuth = new OAuthClient(Config);
    }

    public SpotifyClientConfig CreateForUser()
    {
        return SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(new PKCEAuthenticator(
                _handler.ClientData.ClientId!, new PKCETokenResponse
                {
                    AccessToken = _handler.Token.AccessToken!,
                    CreatedAt = _handler.Token.CreatedAt,
                    ExpiresIn = _handler.Token.ExpiresIn,
                    TokenType = _handler.Token.TokenType!
                }
            ))
            .WithRetryHandler(new SimpleRetryHandler());
    }


    public bool UserLoggedIn([NotNull]out SpotifyClient? spotify)
    {
        if (Spotify == null || Config.Authenticator is not PKCEAuthenticator)
        {
            spotify = null;
            throw new NotLoggedInException("Ur not logged in");
        }

        spotify = Spotify;
        return true;
    }

    public SpotifyClientConfig Config { get; }
    public SpotifyClient? Spotify { get; }
    public OAuthClient OAuth { get; }
}
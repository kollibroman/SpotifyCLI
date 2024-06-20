using System.Diagnostics.CodeAnalysis;
using Core.Exception;
using Core.Helpers;
using Core.Interfaces;
using SpotifyAPI.Web;

namespace Core.Services;

public class SpotifyService
{
    private SpotifyClientConfig _config;
    private SpotifyClient? _spotify;
    private OAuthClient _oauth;
    private readonly DataHandler _handler;
    public SpotifyService(DataHandler handler)
    {
        _handler = handler;

        if (!string.IsNullOrEmpty(_handler.Token.RefreshToken))
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
                _handler.ClientData.ClientId!, new PKCETokenResponse
                {
                    AccessToken = _handler.Token.AccessToken,
                    CreatedAt = _handler.Token.CreatedAt,
                    ExpiresIn = _handler.Token.ExpiresIn,
                    TokenType = _handler.Token.TokenType
                }
            ))
            .WithRetryHandler(new SimpleRetryHandler());
    }


    public bool UserLoggedIn([NotNull]out SpotifyClient? spotify)
    {
        if (Spotify == null || !(Config.Authenticator is PKCEAuthenticator))
        {
            spotify = null;
            throw new NotLoggedInException("Ur not logged in");
        }

        spotify = Spotify;
        return true;
    }

    public SpotifyClientConfig Config { get => _config; }
    public SpotifyClient? Spotify { get => _spotify; }
    public OAuthClient OAuth { get => _oauth; }
}
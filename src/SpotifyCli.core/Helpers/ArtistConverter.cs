using SpotifyCli.Db.Entities;

namespace SpotifyClientCli.Modules;

public class ArtistConverter
{
    public static void ArtistConverter<TInput, TOutput>() where TOutput : SimpleArtists
    {
        
    }
    public Task<List<SimpleArtists>> SpotifySimpleArtistToMine(List<SimpleArtist> artists)
    {
        List<SimpleArtists> artistsList = new();
        artists.ConvertAll<SimpleArtists>();
    }
}
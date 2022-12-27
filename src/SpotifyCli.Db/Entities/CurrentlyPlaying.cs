using System.ComponentModel.DataAnnotations;
using SpotifyAPI.Web;

namespace SpotifyCli.Db.Entities;

public class CurrentlyPlaying
{
    [Key] 
    public int Id { get; set; }
    public string Name { get; set; }
    public List<SimpleArtists> Artists { get; set; }
    public int Duration { get; set; }
}
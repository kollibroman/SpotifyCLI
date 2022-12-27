using System.ComponentModel.DataAnnotations;

namespace SpotifyCli.Db.Entities;

public class SimpleArtists
{
    [Key] 
    public int Id { get; set; }
    public string Href { get; set; } = default!;
    public string ArtistId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string Uri { get; set; } = default!;
}
namespace SpotifyClientCli.Data
{
    public class CurrentlyPlaying
    {
        public string? Name { get; set; }
        public List<SimpleArtist>? Artists { get; set; }
        public int Duration { get; set; }
    }
}
namespace SpotifyClientCli.Data
{
    public class App
    {
        public string? ClientId { get; set; } = File.ReadAllText(AppContext.BaseDirectory + "/ID.txt")!;
        public string? ClientSecret { get; set; } = File.ReadAllText(AppContext.BaseDirectory + "/Secret.txt")!;
    }
}
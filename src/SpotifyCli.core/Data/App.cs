namespace SpotifyClientCli.Data
{
    public class App
    {
        public string? ClientId { get; set; } =  File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/ID.txt")!;
        public string? ClientSecret { get; set; } =  File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Secret.txt")!;
    }
}
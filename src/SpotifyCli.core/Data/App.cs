using System.Net.Mime;
using static System.Net.Mime.MediaTypeNames;

namespace SpotifyClientCli.Data
{
    public class App
    {
        public string? ClientId { get; set; } = File.ReadAllText(AppContext.BaseDirectory + "/ID.txt")!;
    }
}
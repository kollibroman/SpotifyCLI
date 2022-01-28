namespace SpotifyClientCli.Services
{
    public interface ISpotifyBase
    {
        Task OnExecuteAsync(CommandLineApplication app);
    }
}
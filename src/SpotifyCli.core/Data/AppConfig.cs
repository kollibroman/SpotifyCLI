namespace SpotifyClientCli.Data
{
    public class AppConfig 
    {
        public static string AppConfigPath = Path.Combine(
        Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData,
            Environment.SpecialFolderOption.Create
        ),
        "SpotifyCli"
        );

        public string AppConfigFilePath = Path.Combine(
        AppConfigPath,
        "config.json"
        );
        public async Task SaveAsync()
        {
           await File.WriteAllTextAsync(AppConfigFilePath, JsonConvert.SerializeObject(this), Encoding.UTF8);
        }

        public Acc Account { get; } = new();

        public App App { get; } = new();

        public Token Token { get; } = new();
        public Device Device { get; set; } = new();   
    }
}
using System.Text;
using Core.Data;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Core.Services;

namespace Core.Services
{
    public class DataHandler
    {
        private readonly AppConfig _config;

        public DataHandler(AppConfig config)
        {
            _config = config;
        }
        
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

        public ClientData data { get => _config.Data; }
        public UsrAccount account { get => _config.Account; }
        public Token Token { get => _config.Token; }
        public Device Device { get => _config.Device; }
    }
}

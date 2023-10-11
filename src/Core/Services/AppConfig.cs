using System;
using System.Text;
using Core.Data;
using Newtonsoft.Json;

namespace Core.Services
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

        public UsrAccount Account { get; set; } = new();
        public ClientData Data { get; set; } = new();
        public Token Token { get; set; } = new();
        public Device Device { get; set; } = new();  
    }
}

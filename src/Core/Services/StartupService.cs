using System;
using Core.Interfaces;
using Newtonsoft.Json;

namespace Core.Services
{
    public class StartupService
    {
        public async Task<AppConfig> Load()
        {
            AppConfig config = new();

            Directory.CreateDirectory(Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData,
                Environment.SpecialFolderOption.Create),"SpotifyCli"));            

            if (!File.Exists(config.AppConfigFilePath))
            {
               await config.SaveAsync();
               return config;
            }
       
            var configContent = await File.ReadAllTextAsync(config.AppConfigFilePath);
        
            return JsonConvert.DeserializeObject<AppConfig>(configContent)!;
        }
    }
}

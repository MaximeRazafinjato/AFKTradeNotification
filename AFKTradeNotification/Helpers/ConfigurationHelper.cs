using Newtonsoft.Json;

namespace AFKTradeNotification.Helpers;
public class ConfigurationHelper
{
    public static Configuration LoadConfiguration(string configFilePath)
    {
        try
        {
            if (!File.Exists(configFilePath))
            {
                Console.WriteLine("Configuration file not found.");
                Environment.Exit(1);
            }

            var json = File.ReadAllText(configFilePath);
            return JsonConvert.DeserializeObject<Configuration>(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while loading the configuration file : {ex.Message}");
            Environment.Exit(1);
            return null;

        }
    }
}

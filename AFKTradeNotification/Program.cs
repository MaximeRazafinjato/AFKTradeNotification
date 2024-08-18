using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using AFKTradeNotification.Helpers;

class Program
{
    private static string filePath;
    private static string discordWebhookUrl;
    private static string discordId;

    private const string configFilePath = "config.json";
    private const int defaultIntervalDelayInMS = 100;

    static void Main(string[] args)
    {
        LoadConfiguration();

        string lastLine = null;
        List<string> messageToIgnore = new List<string>
        {
            "TalkingPetAudioEvent",
        };

        Console.WriteLine("AFKTradeNotification started successfully! Waiting for incoming messages");

        while (true)
        {
            Thread.Sleep(defaultIntervalDelayInMS);

            string newLine = ReadLastLine(filePath);

            if (newLine == null || newLine == lastLine)
                continue;

            lastLine = newLine;
            if (messageToIgnore.Any(x => lastLine.Contains(x)))
                continue;

            Console.WriteLine($"New line : {lastLine}");

            if (lastLine.Contains("Hi, I would like to buy your"))
            {
                string user = TradeNotificationHelper.GetUserName(lastLine);

                var itemPriceMatch = Regex.Match(lastLine, @"Hi, I would like to buy your (.*?) listed for (\d+\s\w+)");
                string item = itemPriceMatch.Success ? itemPriceMatch.Groups[1].Value.Trim() : TradeNotificationHelper.defaultItem;
                string price = itemPriceMatch.Success ? itemPriceMatch.Groups[2].Value.Trim() : TradeNotificationHelper.defaultPrice;

                string message = $"**New trade for <@{discordId}>:**\n" +
                                 $"- **User:** {user}\n" +
                                 $"- **Item:** {item}\n" +
                                 $"- **Price:** {price}\n" +
                                 $"- **Message:** {Regex.Replace(lastLine, @"^.*@From", "@From")}";

                SendDiscordMessage(discordWebhookUrl, message);
            }
        }
    }

    private static void LoadConfiguration()
    {
        try
        {
            if (!File.Exists(configFilePath))
            {
                Console.WriteLine("Configuration file not found.");
                Environment.Exit(1);
            }

            var json = File.ReadAllText(configFilePath);
            var config = JsonConvert.DeserializeObject<Config>(json);

            filePath = config.FilePath;
            discordWebhookUrl = config.DiscordWebhookUrl;
            discordId = config.DiscordId;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while loading the configuration file : {ex.Message}");
            Environment.Exit(1);
        }
    }

    private static string ReadLastLine(string filePath)
    {
        try
        {
            string lastLine = null;
            string penultimateLine = null;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    penultimateLine = lastLine;
                    lastLine = sr.ReadLine();
                }
            }

            if (lastLine != null && lastLine.Contains("afk", StringComparison.OrdinalIgnoreCase))
                return penultimateLine;

            return lastLine;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while reading the file : {ex.Message}");
            return null;
        }
    }

    private static async void SendDiscordMessage(string webhookUrl, string message)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                var payload = new
                {
                    content = message
                };

                var json = JsonConvert.SerializeObject(payload);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(webhookUrl, data);
                if (response.IsSuccessStatusCode)
                    Console.WriteLine("Notification successfully sent on Discord");
                else
                    Console.WriteLine($"An error occured while sending the Discord notification : {response.StatusCode}");
                    
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while sending the Discord notification : {ex.Message}");
        }
    }
}

using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

class Program
{
    private static string filePath;
    private static string discordWebhookUrl;
    private static string discordId;

    private const string configFilePath = "config.json";
    private const int defaultIntervalDelayInMS = 5000;

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

            Console.WriteLine($"Nouvelle ligne : {lastLine}");

            if (lastLine.Contains("Hi, I would like to buy your"))
            {
                var userMatch = Regex.Match(lastLine, @"\s*([\w\s]+):\s*Hi, I would like to buy your");
                string user = userMatch.Success ? userMatch.Groups[1].Value.Trim() : "Inconnu";

                var itemPriceMatch = Regex.Match(lastLine, @"Hi, I would like to buy your (.*?) listed for (\d+\s\w+)");
                string item = itemPriceMatch.Success ? itemPriceMatch.Groups[1].Value.Trim() : "Inconnu";
                string price = itemPriceMatch.Success ? itemPriceMatch.Groups[2].Value.Trim() : "Inconnu";

                string message = $"**Nouveau trade pour <@{discordId}>:**\n" +
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
                Console.WriteLine("Fichier de configuration non trouvé.");
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
            Console.WriteLine($"Erreur lors du chargement de la configuration : {ex.Message}");
            Environment.Exit(1);
        }
    }

    private static string ReadLastLine(string filePath)
    {
        try
        {
            string lastLine = null;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                    lastLine = sr.ReadLine();
            }
            return lastLine;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la lecture du fichier : {ex.Message}");
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
                    Console.WriteLine("Message envoyé avec succès sur Discord.");
                else
                    Console.WriteLine($"Erreur lors de l'envoi du message Discord : {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'envoi du message Discord : {ex.Message}");
        }
    }
}

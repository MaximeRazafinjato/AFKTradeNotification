using System.Text.RegularExpressions;
using AFKTradeNotification.Helpers;
class Program
{
    private const string configFilePath = "config.json";
    private const int defaultIntervalDelayInMS = 100;

    static void Main(string[] args)
    {
        var config = ConfigurationHelper.LoadConfiguration(configFilePath);

        string lastLine = null;
        List<string> messageToIgnore = new List<string>
        {
            "TalkingPetAudioEvent",
        };

        Console.WriteLine("AFKTradeNotification started successfully! Waiting for incoming messages");

        while (true)
        {
            Thread.Sleep(defaultIntervalDelayInMS);

            string newLine = LogFileHelper.ReadLastLine(config.FilePath);

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

                string message = $"**New trade for <@{config.DiscordId}>:**\n" +
                                 $"- **User:** {user}\n" +
                                 $"- **Item:** {item}\n" +
                                 $"- **Price:** {price}\n" +
                                 $"- **Message:** {Regex.Replace(lastLine, @"^.*@From", "@From")}";

                DiscordHelper.SendDiscordMessage(config.DiscordWebhookUrl, message);
            }
        }
    }
}

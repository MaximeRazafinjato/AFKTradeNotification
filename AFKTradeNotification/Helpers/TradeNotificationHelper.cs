using System.Text.RegularExpressions;

namespace AFKTradeNotification.Helpers;
public class TradeNotificationHelper
{
    public const string defaultUser = "Unknown sender";
    public const string defaultItem = "Unknown item";
    public const string defaultPrice = "Unknown price";

    public static string GetUserName(string lastLine)
    {
        int startIndex = lastLine.IndexOf('@');
        if (startIndex == -1)
        {
            Console.WriteLine("No '@' found in the input string.");
            return defaultUser;
        }

        int firstSpaceIndex = lastLine.IndexOf(' ', startIndex);
        if (firstSpaceIndex == -1)
        {
            Console.WriteLine("No space found after '@'.");
            return defaultUser;
        }

        int endIndex = lastLine.IndexOf(':', firstSpaceIndex);
        if (endIndex == -1)
        {
            Console.WriteLine("No ':' found after '@'.");
            return defaultUser;
        }

        string result = lastLine.Substring(firstSpaceIndex + 1, endIndex - firstSpaceIndex - 1).Trim();
        return result;
    }

    public static (string item, string price) GetItemAndPrice(string lastLine)
    {
        var itemPriceMatch = Regex.Match(lastLine, @"Hi, I would like to buy your (.*?) listed for ([\d\.]+\s\w+)");
        var item = itemPriceMatch.Success ? itemPriceMatch.Groups[1].Value.Trim() : defaultItem;
        var price = itemPriceMatch.Success ? itemPriceMatch.Groups[2].Value.Trim() : defaultPrice;
        return (item, price);
    }
}


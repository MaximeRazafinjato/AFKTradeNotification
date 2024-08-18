using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}


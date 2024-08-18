namespace AFKTradeNotification.Helpers;
public class LogFileHelper
{
    private const string afkString = "afk";
    public static string ReadLastLine(string filePath)
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

            if (lastLine != null && lastLine.Contains(afkString, StringComparison.OrdinalIgnoreCase))
                return penultimateLine;

            return lastLine;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while reading the file : {ex.Message}");
            return null;
        }
    }
}

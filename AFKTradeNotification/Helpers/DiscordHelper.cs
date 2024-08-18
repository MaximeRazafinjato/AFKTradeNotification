using Newtonsoft.Json;
using System.Text;

namespace AFKTradeNotification.Helpers
{
    public class DiscordHelper
    {
        public static async void SendDiscordMessage(string webhookUrl, string message)
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
}

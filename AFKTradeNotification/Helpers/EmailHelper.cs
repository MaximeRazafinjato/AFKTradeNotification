using System.Net;
using System.Net.Mail;

namespace AFKTradeNotification.Helpers;

public class EmailHelper
{
    public static void SendEmail(Configuration config, string subject, string body)
    {
        try
        {
            using (var client = new SmtpClient(config.SmtpServer, config.SmtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(config.SmtpUser, config.SmtpPassword);

                var mailMessage = new MailMessage(config.EmailFrom, config.EmailTo, subject, body);
                client.Send(mailMessage);
                Console.WriteLine("Notification successfully sent by email");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while sending the email notification : {ex.Message}");
        }
    }
}


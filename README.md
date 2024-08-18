# AFKTradeNotification

## Description

The **AFKTradeNotification** is a .NET console application designed to monitor a specific log file for trade requests in Path of Exile. When the application detects a new trade request, it sends a notification via Discord using a webhook. The notifications are customized to include information about the user, the item, and the price of the item.

## Features

- Monitors a log file for new trade requests.
- Extracts relevant information (user, item, price) from the messages.
- Sends a customized notification to a Discord channel when a trade request is detected.
- Uses an external configuration file or environment variables to manage sensitive settings.

## Technologies Used

- **.NET 8**: Framework for building the console application.
- **Newtonsoft.Json**: Library for handling JSON files and serialization/deserialization.
- **Regex**: Regular expressions used to extract relevant information from messages.
- **HttpClient**: Used to send notifications via a Discord webhook.

## Configuration

Before running the application, you need to configure the following settings. You can choose between using an external configuration file or environment variables.

### External Configuration File

**Create the Configuration File**

   Create a file named `config.json` in the same directory as the executable. Example content for `config.json`:

   ```json
   {
       "FilePath": "Path to the log file you want to monitor",
       "DestinationPath": "Path to the temporary file where the log will be copied",
       "DiscordWebhookUrl": "Your Discord webhook URL for sending notifications",
       "DiscordId": "Your Discord user ID for mentions"
   }
   ```

## Obtaining Your Discord ID and Webhook URL

### Enable Developer Mode in Discord

1. Open Discord.
2. Go to User Settings (gear icon at the bottom left).
3. Click on Advanced in the left menu (Under App settings).
4. Enable Developer Mode.

### Obtain Your Discord ID

1. Left-click on your own profile or username in Discord and select "Copy ID".

### Obtain Your Discord Webhook URL

1. Go to your Discord server and click on the settings (gear icon) of the channel where you want to receive notifications.
2. Go to Integrations.
3. Click on Webhooks.
4. Click on "New Webhook" or select an existing one.
5. Copy the Webhook URL from the webhook settings.

### Contributing
If you have suggestions, improvements, or bug fixes, we welcome contributions to improve the AFKTradeNotification project! 

### Authors

- **Maxime Razafinjato**: Project creator
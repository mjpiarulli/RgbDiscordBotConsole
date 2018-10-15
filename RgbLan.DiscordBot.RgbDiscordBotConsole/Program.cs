using System;
using System.Text;
using System.Threading.Tasks;
using RgbLan.DiscordBot.Service;
using RgbLan.DiscordBot.Service.Logger;

namespace RgbLan.DiscordBot.RgbDiscordBotConsole
{
    internal static class Program
    {
        private static async Task Main()
        {
            const string base64ApiKey = "TkRBNU5ETTRPRFF5T1RRNU56TXdNekEwLkRxWjdpdy5OVXIxTm5nS2FNcEFVZjM3ODhXT1BvSFIweVU=";
            var apiKey = Encoding.UTF8.GetString(Convert.FromBase64String(base64ApiKey)); 
            var logger = new ConsoleLogger();
            var bot = new DiscordBotService(apiKey, logger);
            await bot.Start();

            await Task.Delay(-1);
        }
    }
}

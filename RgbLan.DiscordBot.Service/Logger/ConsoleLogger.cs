using System;
using System.Threading.Tasks;
using Discord;
using RgbLan.Discord.Common;

namespace RgbLan.DiscordBot.Service.Logger
{
    public class ConsoleLogger : IDiscordBotLogger
    {
        public Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}

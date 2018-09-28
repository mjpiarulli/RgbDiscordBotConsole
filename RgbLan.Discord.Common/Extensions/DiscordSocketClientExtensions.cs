using System.Threading.Tasks;
using Discord.WebSocket;

namespace RgbLan.Discord.Common.Extensions
{
    public static class DiscordSocketClientExtensions
    {
        public static async Task SendMessageToChannel(this DiscordSocketClient client, ulong channel, string message)
        {
            await (client.GetChannel(channel) as ISocketMessageChannel)?.SendMessageAsync(message);
        }
    }
}

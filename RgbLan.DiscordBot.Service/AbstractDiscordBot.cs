using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace RgbLan.DiscordBot.Service
{
    public abstract class AbstractDiscordBot
    {
        public abstract Task Log(LogMessage msg);
        public abstract Task MessageReceived(SocketMessage message);
        public abstract Task Ready();
        public abstract Task Start();
    }
}

using System.Threading.Tasks;
using Discord;

namespace RgbLan.Discord.Common
{
    public interface IDiscordBotLogger
    {
        Task Log(LogMessage msg);
    }
}

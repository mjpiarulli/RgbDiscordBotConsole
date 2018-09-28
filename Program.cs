using System;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using RgbDiscordBot.Common;
using RgbDiscordBot.DataAccess.Repository;
using RgbDiscordBot.Extensions;

namespace RgbDiscordBotConsole
{
    internal class Program
    {
        private static void Main()
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private DiscordSocketClient _client;

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.MessageReceived += MessageReceived;
            _client.Ready += Ready;


            const string token = "NDA5NDM4ODQyOTQ5NzMwMzA0.DVenQQ.LxJFN0PCvrW5f_b_enCrU1XgT6s"; // Remember to keep this private!
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private static async Task MessageReceived(SocketMessage message)
        {
            var scheduleRepository = new ScheduleRepository();
            //3/2/2018 7:00 PM
            //var time = new DateTime(2018, 3, 2, 18, 59, 0);
            //3/2/2018 10:30 PM
            //var time = new DateTime(2018, 3, 2, 22, 30, 0);
            var time = DateTime.Now;

            if (message.Content == "!rgbhelp")
                await message.Channel.SendMessageAsync(GetRgbHelp());
            if (message.Content == "!whostheworst")
                await message.Channel.SendMessageAsync("Obviously Dave is the worst.  God he's terrible");
            if (message.Content == "!mikesucks")
                await message.Channel.SendMessageAsync("Nuh uh!!");
            if (message.Content == "!nextevent")
            {
                var nextEvent = scheduleRepository.GetEventByNextStartTime(time);
                await message.Channel.SendMessageAsync(nextEvent.ToString());
            }
            if (message.Content == "!nextgameevent")
            {
                var nextEvent = scheduleRepository.GetEventByNextStartTimeAndEventType(time, "Game");
                await message.Channel.SendMessageAsync(nextEvent.ToString());
            }
            if (message.Content == "!nexttabletopevent")
            {
                var nextEvent = scheduleRepository.GetEventByNextStartTimeAndEventType(time, "Tabletop");
                await message.Channel.SendMessageAsync(nextEvent.ToString());
            }

            if (message.Content == "!currentevents")
            {
                var currentEvents = scheduleRepository.GetEventsByNextStartTime(time);
                await message.Channel.SendMessageAsync(currentEvents.ToMoreBetterString());
            }
        }

        private static string GetRgbHelp()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Hello! I'm here to help you with your RGB LAN experience.  Below are the commands I accept.");
            sb.AppendLine();
            sb.AppendLine("!nextevent - gets the next event from the schedule");
            sb.AppendLine("!nextgameevent - gets the next video game event");
            sb.AppendLine("!nexttabletopevent - gets the next table top event");
            sb.AppendLine("!currentevents - get a list of events that are going on right meow");

            return sb.ToString();
        }

        private async Task Ready()
        {
            await SetUpTimedMessages();
        }

        private Task SetUpTimedMessages()
        {
            var timer = new System.Timers.Timer(3600000); // 1 * 60 * 60 * 1000
            //timer.Elapsed += async (sender, e) => await SetUpBeQuietTimedMessages();
            timer.Elapsed += async (sender, e) => await SetUpCandorisMessages();
            timer.Elapsed += async (sender, e) => await SetUpTheGameGuyzMessages();
            timer.Elapsed += async (sender, e) => await SetUpCukUsaMessages();
            timer.Start();

            return Task.CompletedTask;
        }

        private Task SetUpBeQuietTimedMessages()
        {
            (_client.GetChannel(RgbLanRoomConstants.General) as ISocketMessageChannel)?.SendMessageAsync("This is a discount code!");

            return Task.CompletedTask;
        }

        private Task SetUpCandorisMessages()
        {
            (_client.GetChannel(RgbLanRoomConstants.General) as ISocketMessageChannel)?.SendMessageAsync(
                "RGB LAN would like to thank Candoris for being a platinum sponsor of our event! Check them out at https://www.candoris.com/");

            return Task.CompletedTask;
        }

        private Task SetUpTheGameGuyzMessages()
        {
            (_client.GetChannel(RgbLanRoomConstants.General) as ISocketMessageChannel)?.SendMessageAsync("RGB LAN is excited to have The Game Guyz on site running our first tabletop section. Check out their booth on site and get more info at https://www.facebook.com/TheGameGuyz/");

            return Task.CompletedTask;
        }

        private Task SetUpCukUsaMessages()
        {
            (_client.GetChannel(RgbLanRoomConstants.General) as ISocketMessageChannel)?.SendMessageAsync(
                "Computer Upgrade King is one of our awesome platinum sponsors! They offer computer customization and excellent quality PC parts. See more info and deals at https://cukusa.com/");

            return Task.CompletedTask;
        }
    }
}

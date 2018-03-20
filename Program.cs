using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
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
            //await BeQuietTimer();
        }

        private Task SetUpTimedMessages()
        {
            var timer = new System.Timers.Timer(10000); // 1 * 10 * 1000
            timer.Elapsed += async (sender, e) => await SetUpBeQuietTimedMessages();
            timer.Start();

            return Task.CompletedTask;
        }

        private Task SetUpBeQuietTimedMessages()
        {
            (_client.GetChannel(409443383820550174) as ISocketMessageChannel)?.SendMessageAsync("This is a discount code!");

            return Task.CompletedTask;
        }
    }
}

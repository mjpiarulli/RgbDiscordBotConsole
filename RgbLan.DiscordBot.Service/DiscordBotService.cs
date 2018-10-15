using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using Discord.WebSocket;
using RgbLan.Discord.Common;
using RgbLan.Discord.Common.Extensions;
using RgbLan.Lan.DataAccess.Repository;

namespace RgbLan.DiscordBot.Service
{
    public class DiscordBotService : AbstractDiscordBot
    {
        private readonly string _token;
        private readonly IDiscordBotLogger _logger;
        private DiscordSocketClient _client;

        public DiscordBotService(string token, IDiscordBotLogger logger)
        {
            _token = token;
            _logger = logger;
        }

        public override async Task Start()
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;
            _client.MessageReceived += MessageReceived;
            _client.Ready += Ready;

            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        public override async Task Log(LogMessage msg)
        {
            await _logger.Log(msg);
        }

        public override async Task MessageReceived(SocketMessage message)
        {
            var scheduleRepository = new ScheduleRepository();
            
            var time = DateTime.Now;

            if (message.Content == "!rgbhelp")
                await message.Channel.SendMessageAsync(GetRgbHelp());
            if (message.Content == "!whostheworst")
                await message.Channel.SendMessageAsync("Obviously Dave is the worst.  God he's terrible");
            if (message.Content == "!mikesucks")
                await message.Channel.SendMessageAsync("Nuh uh!!");
            if (message.Content == "!nextevent")
            {
                var nextEvents = scheduleRepository.GetEventsByNextStartTime(time);
                if(nextEvents == null)
                    await message.Channel.SendMessageAsync("No next event found :(");
                await message.Channel.SendMessageAsync(nextEvents.ToMoreBetterString());
            }
            if (message.Content == "!nextgameevent")
            {
                var nextEvents = scheduleRepository.GetEventsByNextStartTimeAndEventType(time, "Game");
                if (nextEvents == null)
                    await message.Channel.SendMessageAsync("No next event found :(");
                await message.Channel.SendMessageAsync(nextEvents.ToMoreBetterString());
            }
            if (message.Content == "!nexttabletopevent")
            {
                var nextEvents = scheduleRepository.GetEventsByNextStartTimeAndEventType(time, "Tabletop");
                if (nextEvents == null)
                    await message.Channel.SendMessageAsync("No next event found :(");
                await message.Channel.SendMessageAsync(nextEvents.ToMoreBetterString());
            }

            if (message.Content == "!currentevents")
            {
                var currentEvents = scheduleRepository.GetCurrentEvents(time);
                if (currentEvents == null)
                    await message.Channel.SendMessageAsync("No current event found :(");
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

        public override async Task Ready()
        {
            await SetUpTimedMessages();
        }

        private Task SetUpTimedMessages()
        {
            var sponsorMessageDict = GetSponsorMessages()
                .GroupBy(tm => tm.MessageRepeatMilliseconds)
                .Select(tm => new
                {
                    tm.Key,
                    TimedMessages = tm.ToList()
                }).ToList();

            var timers = new List<Timer>();
            foreach (var key in sponsorMessageDict)
            {
                var timer = new Timer(key.Key);
                foreach (var timedMessage in key.TimedMessages)
                    timer.Elapsed += async (sender, e) => await _client.SendMessageToChannel(timedMessage.Channel, timedMessage.Message);

                timers.Add(timer);
            }
            
            timers.ForEach(t => t.Start());

            return Task.CompletedTask;
        }

        private static List<TimedMessageDto> GetSponsorMessages()
        {
            const string beQuietMessage = "This is a discount code!";
            const string candorisMessage = "RGB LAN would like to thank Candoris for being a platinum sponsor of our event! Check them out at https://www.candoris.com/";
            const string theGameGuyzMessage = "RGB LAN is excited to have The Game Guyz on site running our first tabletop section. Check out their booth on site and get more info at https://www.facebook.com/TheGameGuyz/";
            const string cukUsaMessage = "Computer Upgrade King is one of our awesome platinum sponsors! They offer computer customization and excellent quality PC parts. See more info and deals at https://cukusa.com/";
            const ulong room = RgbLanRoomConstants.Dev;
            const int timeMilli = 300000;

            return new List<TimedMessageDto>
            {
                new TimedMessageDto(room, beQuietMessage, timeMilli),
                new TimedMessageDto(room, candorisMessage, timeMilli),
                new TimedMessageDto(room, theGameGuyzMessage, timeMilli),
                new TimedMessageDto(room, cukUsaMessage, timeMilli)
            };
        }
    }
}

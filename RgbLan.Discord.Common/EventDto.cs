using System;

namespace RgbLan.Discord.Common
{
    public class EventDto
    {
        public EventDto()
        {
            EventId = 0;
            Title = string.Empty;
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
            EventType = string.Empty;
        }

        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string EventType { get; set; }

        public override string ToString()
        {
            return $"{Title} ({EventType}) Start time: {StartTime:g} End time: {EndTime:g}";
        }
    }
}

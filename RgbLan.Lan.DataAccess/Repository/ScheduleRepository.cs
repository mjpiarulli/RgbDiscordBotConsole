using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Json;
using RgbLan.Discord.Common;
using RgbLan.Lan.DataAccess.Interface;

namespace RgbLan.Lan.DataAccess.Repository
{
    public class ScheduleRepository : GenericJsonFileRepository<EventDto>, IScheduleRepository
    {
        public List<EventDto> GetEventsByNextStartTime(DateTime startTime)
        {
            var nextEvent = FindBy(e => e.StartTime >= startTime).FirstOrDefault();
            var events = FindBy(e => e.StartTime == nextEvent.StartTime).ToList();
            return events;
        }

        public List<EventDto> GetEventsByNextStartTimeAndEventType(DateTime startTime, string eventType)
        {
            var nextEvent = FindBy(e => e.StartTime >= startTime && e.EventType == eventType).FirstOrDefault();
            var events = FindBy(e => e.StartTime == nextEvent.StartTime && e.EventType == eventType).ToList();
            return events;
        }

        public List<EventDto> GetCurrentEvents(DateTime now)
        {
            var events = FindBy(e => e.StartTime <= now && e.EndTime >= now).ToList();
            return events;
        }
    }
}

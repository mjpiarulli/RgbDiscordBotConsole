using System;
using System.Collections.Generic;
using DataAccess;
using RgbLan.Discord.Common;

namespace RgbLan.Lan.DataAccess.Interface
{
    public interface IScheduleRepository : IGenericRepository<EventDto, int>
    {
        List<EventDto> GetEventsByNextStartTime(DateTime startTime);
        List<EventDto> GetEventsByNextStartTimeAndEventType(DateTime startTime, string eventType);
        List<EventDto> GetCurrentEvents(DateTime startTime);
    }
}

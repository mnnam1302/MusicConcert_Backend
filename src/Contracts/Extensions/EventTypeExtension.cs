using Contracts.Enumerations;

namespace Contracts.Extensions;

public static class EventTypeExtension
{
    /// <summary>
    /// Online or Onlineee..... => EventType.Online
    /// 
    /// </summary>
    /// <param name="eventType"></param>
    /// <returns></returns>
    public static EventType ConvertStringToEventType(string eventType)
    {
        return eventType.ToUpper().Trim().StartsWith("ONLINE")
            ? EventType.Online : EventType.Offline;
    }
}
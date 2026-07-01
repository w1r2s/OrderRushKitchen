using System.Collections.Generic;

namespace OrderRushKitchen.Sdk
{
    public interface IAnalyticsService
    {
        void LogEvent(string eventName);
        void LogEvent(string eventName, IReadOnlyDictionary<string, object> parameters);
    }
}
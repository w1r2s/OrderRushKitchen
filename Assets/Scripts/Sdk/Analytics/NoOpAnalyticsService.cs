using System.Collections.Generic;

namespace OrderRushKitchen.Sdk
{
    public sealed class NoOpAnalyticsService : IAnalyticsService
    {
        public void LogEvent(string eventName)
        {
        }

        public void LogEvent(string eventName, IReadOnlyDictionary<string, object> parameters)
        {
        }
    }
}

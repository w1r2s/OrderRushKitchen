using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OrderRushKitchen.Sdk
{
    public sealed class DebugAnalyticsService : IAnalyticsService
    {
        private readonly SdkSettings _settings;

        public DebugAnalyticsService(SdkSettings settings)
        {
            _settings = settings;
        }

        public void LogEvent(string eventName)
        {
            LogEvent(eventName, null);
        }

        public void LogEvent(string eventName, IReadOnlyDictionary<string, object> parameters)
        {
            if (_settings == null || !_settings.verboseSdkLogging)
            {
                return;
            }

            string payload = parameters == null || parameters.Count == 0
                ? string.Empty
                : " " + string.Join(", ", parameters.Select(parameter => $"{parameter.Key}={parameter.Value}"));

            Debug.Log($"[Analytics:Debug] {eventName}{payload}");
        }
    }
}
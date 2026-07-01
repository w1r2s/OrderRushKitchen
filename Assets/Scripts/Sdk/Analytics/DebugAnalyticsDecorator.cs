using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OrderRushKitchen.Sdk
{
    public sealed class DebugAnalyticsDecorator : IAnalyticsService
    {
        private readonly IAnalyticsService _inner;
        private readonly SdkSettings _settings;

        public DebugAnalyticsDecorator(IAnalyticsService inner, SdkSettings settings)
        {
            _inner = inner;
            _settings = settings;
        }

        public void LogEvent(string eventName)
        {
            LogEvent(eventName, null);
        }

        public void LogEvent(string eventName, IReadOnlyDictionary<string, object> parameters)
        {
            if (_settings != null && _settings.verboseSdkLogging)
            {
                Debug.Log($"[Analytics:Debug] {eventName}{FormatParameters(CreateContextualLogParameters(parameters))}");
            }

            _inner.LogEvent(eventName, parameters);
        }

        private Dictionary<string, object> CreateContextualLogParameters(IReadOnlyDictionary<string, object> parameters)
        {
            Dictionary<string, object> contextualParameters = parameters == null
                ? new Dictionary<string, object>()
                : new Dictionary<string, object>(parameters);

            if (!contextualParameters.ContainsKey(AnalyticsParameters.Environment))
            {
                contextualParameters[AnalyticsParameters.Environment] = SdkRuntimeContext.GetEnvironmentName(_settings);
            }

            if (!contextualParameters.ContainsKey(AnalyticsParameters.Platform))
            {
                contextualParameters[AnalyticsParameters.Platform] = SdkRuntimeContext.GetPlatformName();
            }

            if (!contextualParameters.ContainsKey(AnalyticsParameters.AppVersion))
            {
                contextualParameters[AnalyticsParameters.AppVersion] = SdkRuntimeContext.GetAppVersion();
            }

            if (!contextualParameters.ContainsKey(AnalyticsParameters.BuildType))
            {
                contextualParameters[AnalyticsParameters.BuildType] = SdkRuntimeContext.GetBuildType();
            }

            return contextualParameters;
        }

        private static string FormatParameters(IReadOnlyDictionary<string, object> parameters)
        {
            return parameters == null || parameters.Count == 0
                ? string.Empty
                : " " + string.Join(", ", parameters.Select(parameter => $"{parameter.Key}={parameter.Value}"));
        }
    }
}

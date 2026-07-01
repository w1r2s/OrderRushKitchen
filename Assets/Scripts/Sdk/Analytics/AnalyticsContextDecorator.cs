using System.Collections.Generic;

namespace OrderRushKitchen.Sdk
{
    public sealed class AnalyticsContextDecorator : IAnalyticsService
    {
        private readonly IAnalyticsService _inner;
        private readonly SdkSettings _settings;

        public AnalyticsContextDecorator(IAnalyticsService inner, SdkSettings settings)
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
            _inner.LogEvent(eventName, CreateContextualParameters(parameters));
        }

        private Dictionary<string, object> CreateContextualParameters(IReadOnlyDictionary<string, object> parameters)
        {
            Dictionary<string, object> contextualParameters = parameters == null
                ? new Dictionary<string, object>()
                : new Dictionary<string, object>(parameters);

            contextualParameters[AnalyticsParameters.Environment] = SdkRuntimeContext.GetEnvironmentName(_settings);
            contextualParameters[AnalyticsParameters.Platform] = SdkRuntimeContext.GetPlatformName();
            contextualParameters[AnalyticsParameters.AppVersion] = SdkRuntimeContext.GetAppVersion();
            contextualParameters[AnalyticsParameters.BuildType] = SdkRuntimeContext.GetBuildType();

            return contextualParameters;
        }
    }
}

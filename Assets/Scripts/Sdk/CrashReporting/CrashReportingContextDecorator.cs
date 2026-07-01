using System;
using System.Collections.Generic;

namespace OrderRushKitchen.Sdk
{
    public sealed class CrashReportingContextDecorator : ICrashReportingService
    {
        private readonly ICrashReportingService _inner;
        private readonly SdkSettings _settings;

        public CrashReportingContextDecorator(ICrashReportingService inner, SdkSettings settings)
        {
            _inner = inner;
            _settings = settings;
        }

        public void LogBreadcrumb(string message)
        {
            _inner.LogBreadcrumb(message);
        }

        public void SetCustomKey(string key, object value)
        {
            _inner.SetCustomKey(key, value);
        }

        public void ReportException(Exception exception)
        {
            ReportException(exception, null);
        }

        public void ReportException(Exception exception, IReadOnlyDictionary<string, object> context)
        {
            _inner.ReportException(exception, CreateContextualCrashData(context));
        }

        private Dictionary<string, object> CreateContextualCrashData(IReadOnlyDictionary<string, object> context)
        {
            Dictionary<string, object> contextualData = context == null
                ? new Dictionary<string, object>()
                : new Dictionary<string, object>(context);

            contextualData[CrashReportKeys.Environment] = SdkRuntimeContext.GetEnvironmentName(_settings);
            contextualData[CrashReportKeys.Platform] = SdkRuntimeContext.GetPlatformName();
            contextualData[CrashReportKeys.AppVersion] = SdkRuntimeContext.GetAppVersion();
            contextualData[CrashReportKeys.BuildType] = SdkRuntimeContext.GetBuildType();

            return contextualData;
        }
    }
}

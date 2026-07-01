using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OrderRushKitchen.Sdk
{
    public sealed class DebugCrashReportingDecorator : ICrashReportingService
    {
        private readonly ICrashReportingService _inner;
        private readonly SdkSettings _settings;

        public DebugCrashReportingDecorator(ICrashReportingService inner, SdkSettings settings)
        {
            _inner = inner;
            _settings = settings;
        }

        public void LogBreadcrumb(string message)
        {
            if (ShouldLog())
            {
                Debug.Log($"[CrashReporting:Debug] breadcrumb {message}");
            }

            _inner.LogBreadcrumb(message);
        }

        public void SetCustomKey(string key, object value)
        {
            if (ShouldLog())
            {
                Debug.Log($"[CrashReporting:Debug] key {key}={value}");
            }

            _inner.SetCustomKey(key, value);
        }

        public void ReportException(Exception exception)
        {
            ReportException(exception, null);
        }

        public void ReportException(Exception exception, IReadOnlyDictionary<string, object> context)
        {
            if (ShouldLog() && exception != null)
            {
                Debug.LogWarning($"[CrashReporting:Debug] non_fatal {exception.GetType().Name}: {exception.Message}{FormatContext(CreateContextualLogData(context))}");
            }

            _inner.ReportException(exception, context);
        }

        private bool ShouldLog()
        {
            return _settings != null && _settings.verboseSdkLogging;
        }

        private Dictionary<string, object> CreateContextualLogData(IReadOnlyDictionary<string, object> context)
        {
            Dictionary<string, object> contextualData = context == null
                ? new Dictionary<string, object>()
                : new Dictionary<string, object>(context);

            if (!contextualData.ContainsKey(CrashReportKeys.Environment))
            {
                contextualData[CrashReportKeys.Environment] = SdkRuntimeContext.GetEnvironmentName(_settings);
            }

            if (!contextualData.ContainsKey(CrashReportKeys.Platform))
            {
                contextualData[CrashReportKeys.Platform] = SdkRuntimeContext.GetPlatformName();
            }

            if (!contextualData.ContainsKey(CrashReportKeys.AppVersion))
            {
                contextualData[CrashReportKeys.AppVersion] = SdkRuntimeContext.GetAppVersion();
            }

            if (!contextualData.ContainsKey(CrashReportKeys.BuildType))
            {
                contextualData[CrashReportKeys.BuildType] = SdkRuntimeContext.GetBuildType();
            }

            return contextualData;
        }

        private static string FormatContext(IReadOnlyDictionary<string, object> context)
        {
            return context == null || context.Count == 0
                ? string.Empty
                : " " + string.Join(", ", context.Select(pair => $"{pair.Key}={pair.Value}"));
        }
    }
}

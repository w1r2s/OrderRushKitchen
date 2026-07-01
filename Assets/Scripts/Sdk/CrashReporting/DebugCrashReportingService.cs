using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OrderRushKitchen.Sdk
{
    public sealed class DebugCrashReportingService : ICrashReportingService
    {
        private readonly SdkSettings _settings;

        public DebugCrashReportingService(SdkSettings settings)
        {
            _settings = settings;
        }

        public void LogBreadcrumb(string message)
        {
            if (!ShouldLog())
            {
                return;
            }

            Debug.Log($"[CrashReporting:Debug] breadcrumb {message}");
        }

        public void SetCustomKey(string key, object value)
        {
            if (!ShouldLog())
            {
                return;
            }

            Debug.Log($"[CrashReporting:Debug] key {key}={value}");
        }

        public void ReportException(Exception exception)
        {
            ReportException(exception, null);
        }

        public void ReportException(Exception exception, IReadOnlyDictionary<string, object> context)
        {
            if (!ShouldLog() || exception == null)
            {
                return;
            }

            string payload = FormatContext(context);
            Debug.LogWarning($"[CrashReporting:Debug] non_fatal {exception.GetType().Name}: {exception.Message}{payload}");
        }

        private bool ShouldLog()
        {
            return _settings != null && _settings.verboseSdkLogging;
        }

        private static string FormatContext(IReadOnlyDictionary<string, object> context)
        {
            if (context == null || context.Count == 0)
            {
                return string.Empty;
            }

            return " " + string.Join(", ", context.Select(pair => $"{pair.Key}={pair.Value}"));
        }
    }
}
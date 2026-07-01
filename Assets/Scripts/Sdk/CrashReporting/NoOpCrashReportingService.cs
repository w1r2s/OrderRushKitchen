using System;
using System.Collections.Generic;

namespace OrderRushKitchen.Sdk
{
    public sealed class NoOpCrashReportingService : ICrashReportingService
    {
        public void LogBreadcrumb(string message)
        {
        }

        public void SetCustomKey(string key, object value)
        {
        }

        public void ReportException(Exception exception)
        {
        }

        public void ReportException(Exception exception, IReadOnlyDictionary<string, object> context)
        {
        }
    }
}

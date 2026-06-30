using System;
using System.Collections.Generic;

namespace OrderRushKitchen.Sdk
{
    public interface ICrashReportingService
    {
        void LogBreadcrumb(string message);
        void SetCustomKey(string key, object value);
        void ReportException(Exception exception);
        void ReportException(Exception exception, IReadOnlyDictionary<string, object> context);
    }
}
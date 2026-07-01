using Firebase.Crashlytics;
using System;
using System.Collections.Generic;
using System.Globalization;
using Zenject;

namespace OrderRushKitchen.Sdk
{
    public sealed class FirebaseCrashReportingService : ICrashReportingService, IInitializable, IDisposable
    {
        private const int MaxPendingReports = 16;

        private readonly ISdkInitializationService _sdkInitializationService;
        private readonly SdkSettings _settings;
        private readonly Queue<PendingCrashReport> _pendingReports = new Queue<PendingCrashReport>();

        public FirebaseCrashReportingService(ISdkInitializationService sdkInitializationService, SdkSettings settings)
        {
            _sdkInitializationService = sdkInitializationService;
            _settings = settings;
        }

        public void Initialize()
        {
            _sdkInitializationService.StateChanged += OnSdkStateChanged;

            if (_sdkInitializationService.IsReady)
            {
                FlushPendingReports();
            }
        }

        public void Dispose()
        {
            _sdkInitializationService.StateChanged -= OnSdkStateChanged;
        }

        public void LogBreadcrumb(string message)
        {
            if (!CanUseCrashlytics())
            {
                return;
            }

            Crashlytics.Log(message);
        }

        public void SetCustomKey(string key, object value)
        {
            if (!CanUseCrashlytics())
            {
                return;
            }

            SetFirebaseCustomKey(key, value);
        }

        public void ReportException(Exception exception)
        {
            ReportException(exception, null);
        }

        public void ReportException(Exception exception, IReadOnlyDictionary<string, object> context)
        {
            if (_settings == null || !_settings.crashlyticsEnabled || exception == null)
            {
                return;
            }

            if (_sdkInitializationService.IsReady)
            {
                SendReport(exception, context);
                return;
            }

            QueuePendingReport(exception, context);
        }

        private void OnSdkStateChanged(SdkInitializationState state)
        {
            if (state == SdkInitializationState.Ready)
            {
                FlushPendingReports();
                return;
            }

            if (state == SdkInitializationState.Failed || state == SdkInitializationState.Disabled)
            {
                _pendingReports.Clear();
            }
        }

        private bool CanUseCrashlytics()
        {
            return _settings != null && _settings.crashlyticsEnabled && _sdkInitializationService.IsReady;
        }

        private void QueuePendingReport(Exception exception, IReadOnlyDictionary<string, object> context)
        {
            if (_pendingReports.Count >= MaxPendingReports)
            {
                _pendingReports.Dequeue();
            }

            _pendingReports.Enqueue(new PendingCrashReport(exception, CopyContext(context)));
        }

        private void FlushPendingReports()
        {
            while (_pendingReports.Count > 0)
            {
                PendingCrashReport report = _pendingReports.Dequeue();
                SendReport(report.Exception, report.Context);
            }
        }

        private static void SendReport(Exception exception, IReadOnlyDictionary<string, object> context)
        {
            if (context != null)
            {
                foreach (KeyValuePair<string, object> pair in context)
                {
                    SetFirebaseCustomKey(pair.Key, pair.Value);
                }
            }

            Crashlytics.LogException(exception);
        }

        private static Dictionary<string, object> CopyContext(IReadOnlyDictionary<string, object> context)
        {
            return context == null
                ? null
                : new Dictionary<string, object>(context);
        }

        private static void SetFirebaseCustomKey(string key, object value)
        {
            Crashlytics.SetCustomKey(key, ToCrashlyticsValue(value));
        }

        private static string ToCrashlyticsValue(object value)
        {
            return value switch
            {
                null => string.Empty,
                float floatValue => floatValue.ToString(CultureInfo.InvariantCulture),
                double doubleValue => doubleValue.ToString(CultureInfo.InvariantCulture),
                decimal decimalValue => decimalValue.ToString(CultureInfo.InvariantCulture),
                bool boolValue => boolValue ? "true" : "false",
                _ => value.ToString()
            };
        }

        private readonly struct PendingCrashReport
        {
            public readonly Exception Exception;
            public readonly IReadOnlyDictionary<string, object> Context;

            public PendingCrashReport(Exception exception, IReadOnlyDictionary<string, object> context)
            {
                Exception = exception;
                Context = context;
            }
        }
    }
}
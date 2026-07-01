using Firebase.Analytics;
using System;
using System.Collections.Generic;
using Zenject;

namespace OrderRushKitchen.Sdk
{
    public sealed class FirebaseAnalyticsService : IAnalyticsService, IInitializable, IDisposable
    {
        private const int MaxPendingEvents = 32;

        private readonly ISdkInitializationService _sdkInitializationService;
        private readonly SdkSettings _settings;
        private readonly Queue<PendingAnalyticsEvent> _pendingEvents = new Queue<PendingAnalyticsEvent>();

        public FirebaseAnalyticsService(ISdkInitializationService sdkInitializationService, SdkSettings settings)
        {
            _sdkInitializationService = sdkInitializationService;
            _settings = settings;
        }

        public void Initialize()
        {
            _sdkInitializationService.StateChanged += OnSdkStateChanged;

            if (_sdkInitializationService.IsReady)
            {
                FlushPendingEvents();
            }
        }

        public void Dispose()
        {
            _sdkInitializationService.StateChanged -= OnSdkStateChanged;
        }

        public void LogEvent(string eventName)
        {
            LogEvent(eventName, null);
        }

        public void LogEvent(string eventName, IReadOnlyDictionary<string, object> parameters)
        {
            if (_settings == null || !_settings.analyticsEnabled)
            {
                return;
            }

            if (_sdkInitializationService.IsReady)
            {
                SendEvent(eventName, parameters);
                return;
            }

            QueuePendingEvent(eventName, parameters);
        }

        private void OnSdkStateChanged(SdkInitializationState state)
        {
            if (state == SdkInitializationState.Ready)
            {
                FlushPendingEvents();
                return;
            }

            if (state == SdkInitializationState.Failed || state == SdkInitializationState.Disabled)
            {
                _pendingEvents.Clear();
            }
        }

        private void QueuePendingEvent(string eventName, IReadOnlyDictionary<string, object> parameters)
        {
            if (_pendingEvents.Count >= MaxPendingEvents)
            {
                _pendingEvents.Dequeue();
            }

            _pendingEvents.Enqueue(new PendingAnalyticsEvent(eventName, CopyParameters(parameters)));
        }

        private void FlushPendingEvents()
        {
            while (_pendingEvents.Count > 0)
            {
                PendingAnalyticsEvent pendingEvent = _pendingEvents.Dequeue();
                SendEvent(pendingEvent.EventName, pendingEvent.Parameters);
            }
        }

        private static Dictionary<string, object> CopyParameters(IReadOnlyDictionary<string, object> parameters)
        {
            return parameters == null
                ? null
                : new Dictionary<string, object>(parameters);
        }

        private static void SendEvent(string eventName, IReadOnlyDictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                FirebaseAnalytics.LogEvent(eventName);
                return;
            }

            FirebaseAnalytics.LogEvent(eventName, ToFirebaseParameters(parameters));
        }

        private static Parameter[] ToFirebaseParameters(IReadOnlyDictionary<string, object> parameters)
        {
            var firebaseParameters = new Parameter[parameters.Count];
            int index = 0;

            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                firebaseParameters[index] = ToFirebaseParameter(parameter.Key, parameter.Value);
                index++;
            }

            return firebaseParameters;
        }

        private static Parameter ToFirebaseParameter(string key, object value)
        {
            return value switch
            {
                int intValue => new Parameter(key, intValue),
                long longValue => new Parameter(key, longValue),
                float floatValue => new Parameter(key, floatValue),
                double doubleValue => new Parameter(key, doubleValue),
                bool boolValue => new Parameter(key, boolValue ? "true" : "false"),
                null => new Parameter(key, string.Empty),
                _ => new Parameter(key, value.ToString())
            };
        }

        private readonly struct PendingAnalyticsEvent
        {
            public readonly string EventName;
            public readonly IReadOnlyDictionary<string, object> Parameters;

            public PendingAnalyticsEvent(string eventName, IReadOnlyDictionary<string, object> parameters)
            {
                EventName = eventName;
                Parameters = parameters;
            }
        }
    }
}
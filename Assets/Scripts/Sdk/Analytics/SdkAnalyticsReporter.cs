using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Sdk
{
    public sealed class SdkAnalyticsReporter : IInitializable, IDisposable
    {
        private readonly ISdkInitializationService _sdkInitializationService;
        private readonly IAnalyticsService _analyticsService;
        private readonly SdkSettings _settings;

        public SdkAnalyticsReporter(ISdkInitializationService sdkInitializationService, IAnalyticsService analyticsService, SdkSettings settings)
        {
            _sdkInitializationService = sdkInitializationService;
            _analyticsService = analyticsService;
            _settings = settings;
        }

        public void Initialize()
        {
            _sdkInitializationService.StateChanged += OnSdkStateChanged;

            _analyticsService.LogEvent(AnalyticsEvents.AppStart, CreateBaseParameters());

            if (_sdkInitializationService.State == SdkInitializationState.Ready ||
                _sdkInitializationService.State == SdkInitializationState.Failed)
            {
                OnSdkStateChanged(_sdkInitializationService.State);
            }
        }

        public void Dispose()
        {
            _sdkInitializationService.StateChanged -= OnSdkStateChanged;
        }

        private void OnSdkStateChanged(SdkInitializationState state)
        {
            if (state == SdkInitializationState.Ready)
            {
                _analyticsService.LogEvent(AnalyticsEvents.FirebaseInitSucceeded, CreateBaseParameters());
                return;
            }

            if (state == SdkInitializationState.Failed)
            {
                Dictionary<string, object> parameters = CreateBaseParameters();
                parameters[AnalyticsParameters.ErrorStage] = "dependency_check";
                parameters[AnalyticsParameters.ErrorCode] = _sdkInitializationService.ErrorMessage;

                _analyticsService.LogEvent(AnalyticsEvents.FirebaseInitFailed, parameters);
            }
        }

        private Dictionary<string, object> CreateBaseParameters()
        {
            string environment = _settings != null ? _settings.environment.ToString().ToLowerInvariant() : "unknown";

            return new Dictionary<string, object>
            {
                [AnalyticsParameters.Environment] = environment,
                [AnalyticsParameters.Platform] = Application.platform.ToString().ToLowerInvariant(),
                [AnalyticsParameters.AppVersion] = Application.version
            };
        }
    }
}
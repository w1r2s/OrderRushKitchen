using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Sdk
{
    public sealed class SdkCrashReporter : IInitializable, IDisposable
    {
        private readonly ISdkInitializationService _sdkInitializationService;
        private readonly ICrashReportingService _crashReportingService;
        private readonly SdkSettings _settings;

        public SdkCrashReporter(ISdkInitializationService sdkInitializationService, ICrashReportingService crashReportingService, SdkSettings settings)
        {
            _sdkInitializationService = sdkInitializationService;
            _crashReportingService = crashReportingService;
            _settings = settings;
        }

        public void Initialize()
        {
            _sdkInitializationService.StateChanged += OnSdkStateChanged;

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
            _crashReportingService.SetCustomKey(CrashReportKeys.SdkState, state.ToString());

            if (state != SdkInitializationState.Failed)
            {
                return;
            }

            var context = new Dictionary<string, object>
            {
                [CrashReportKeys.Environment] = _settings != null ? _settings.environment.ToString().ToLowerInvariant() : "unknown",
                [CrashReportKeys.Platform] = Application.platform.ToString().ToLowerInvariant(),
                [CrashReportKeys.AppVersion] = Application.version,
                [CrashReportKeys.SdkState] = state.ToString(),
                [CrashReportKeys.ErrorStage] = "firebase_initialization"
            };

            string errorMessage = string.IsNullOrWhiteSpace(_sdkInitializationService.ErrorMessage)
                ? "SDK initialization failed."
                : _sdkInitializationService.ErrorMessage;

            _crashReportingService.ReportException(new InvalidOperationException(errorMessage), context);
        }
    }
}
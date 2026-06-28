using Firebase;
using Firebase.Analytics;
using Firebase.Crashlytics;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Sdk
{
    public sealed class SdkInitializationService : ISdkInitializationService, IInitializable
    {
        private readonly SdkSettings _settings;
        public SdkInitializationState State { get; private set; } = SdkInitializationState.NotStarted;
        public bool IsReady => State == SdkInitializationState.Ready;
        public bool IsDisabled => State == SdkInitializationState.Disabled;
        public string ErrorMessage { get; private set; } = string.Empty;

        public event Action<SdkInitializationState> StateChanged;

        public SdkInitializationService(SdkSettings settings)
        {
            _settings = settings;
        }

        public void Initialize()
        {
            if (State != SdkInitializationState.NotStarted)
            {
                return;
            }

            if (_settings == null)
            {
                Fail("SdkSettings is not assigned.");
                return;
            }
            if (!_settings.sdkEnabled || !_settings.firebaseEnabled)
            {
                ErrorMessage = string.Empty;
                SetState(SdkInitializationState.Disabled);
                Log("Firebase initialization disabled by settings.");
                return;
            }

            SetState(SdkInitializationState.Initializing);
            InitializeFirebase();
        }

        private void SetState(SdkInitializationState state)
        {
            if (State == state)
            {
                return;
            }

            State = state;
            StateChanged?.Invoke(State);
        }

        private void InitializeFirebase()
        {
            Log("Firebase dependency check started.");

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(HandleFirebaseDependencyCheckCompleted);
        }

        private void HandleFirebaseDependencyCheckCompleted(Task<DependencyStatus> task)
        {
            if (task.IsCanceled)
            {
                Fail("Firebase dependency check was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                string message = task.Exception?.GetBaseException().Message ?? "Unknown Firebase dependency check exception.";
                Fail($"Firebase dependency check failed: {message}");
                return;
            }

            DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus != DependencyStatus.Available)
            {
                Fail($"Firebase dependencies are not available: {dependencyStatus}");
                return;
            }

            FirebaseAnalytics.SetAnalyticsCollectionEnabled(_settings.analyticsEnabled);
            Crashlytics.IsCrashlyticsCollectionEnabled = _settings.crashlyticsEnabled;

            ErrorMessage = string.Empty;
            SetState(SdkInitializationState.Ready);
            Log("Firebase initialized.");
        }

        private void Fail(string errorMessage)
        {
            ErrorMessage = errorMessage;
            SetState(SdkInitializationState.Failed);
            Debug.LogError($"[SDK] {errorMessage}");
        }

        private void Log(string message)
        {
            if (_settings != null && _settings.verboseSdkLogging)
            {
                Debug.Log($"[SDK] {message}");
            }
        }
    }
}

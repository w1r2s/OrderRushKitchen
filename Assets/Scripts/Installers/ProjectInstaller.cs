using OrderRushKitchen.Audio;
using OrderRushKitchen.Input;
using OrderRushKitchen.Level;
using OrderRushKitchen.Localization;
using OrderRushKitchen.Navigation;
using OrderRushKitchen.Sdk;
using OrderRushKitchen.UserProgress;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [Header("Levels")]
        [SerializeField] private LevelDefinitionListSo levelDefinitionListSo;

        [Header("Music")]
        [SerializeField] private MusicTrackLibrarySo musicTrackLibrary;

        [Header("SDK")]
        [SerializeField] private SdkSettings sdkSettings;

        public override void InstallBindings()
        {
            // Navigation
            Container.Bind<ISceneLoader>().To<UnitySceneLoader>().AsSingle();
            Container.Decorate<ISceneLoader>().With<SceneLoadAnalyticsDecorator>();
            Container.Bind<ILoadingScreen>().FromComponentInHierarchy().AsSingle();
            Container.Bind<INavigationService>().To<NavigationService>().AsSingle();

            // Input
            Container.Bind<Actions>().AsSingle();

            Container.Bind<IInputStorage>().To<PlayerPrefsInputStorage>().AsSingle();
            Container.BindInterfacesTo<InputRebindingService>().AsSingle();

            // Audio settings
            Container.Bind<IAudioSettingsStorage>().To<PlayerPrefsAudioSettingsStorage>().AsSingle();
            Container.Bind<IAudioSettingsService>().To<AudioSettingsService>().AsSingle();

            // Music
            Container.BindInstance(musicTrackLibrary);
            Container.Bind<IMusicPlayer>().To<MusicPlayer>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IMusicService>().To<MusicService>().AsSingle();
            Container.BindInterfacesTo<MusicVolumeController>().AsSingle();

            // User progress
            Container.Bind<IUserProgressStorage>().To<PlayerPrefsUserProgressStorage>().AsSingle();
            Container.BindInterfacesTo<UserProgressService>().AsSingle();

            // Level configuration
            Container.Bind<LevelDatabase>().AsSingle().WithArguments(levelDefinitionListSo.levels);

            // Localization
            Container.Bind<ILocalizationStorage>().To<PlayerPrefsLocalizationStorage>().AsSingle();
            Container.BindInterfacesTo<LocalizationService>().AsSingle();

            // SDK
            BindSdkServices();
        }

        private void BindSdkServices()
        {
            Container.BindInstance(sdkSettings);
            Container.BindInterfacesTo<SdkInitializationService>().AsSingle();

            BindAnalyticsService();
            BindCrashReportingService();

            Container.BindInterfacesTo<SdkAnalyticsReporter>().AsSingle();
            Container.BindInterfacesTo<SdkCrashReporter>().AsSingle();
        }

        private void BindAnalyticsService()
        {
            if (!IsAnalyticsEnabled())
            {
                Container.Bind<IAnalyticsService>().To<NoOpAnalyticsService>().AsSingle();
                return;
            }

            bool useFirebase = IsFirebaseEnabled();

            if (useFirebase)
            {
                Container.BindInterfacesTo<FirebaseAnalyticsService>().AsSingle();
            }
            else if (IsVerboseSdkLoggingEnabled())
            {
                Container.Bind<IAnalyticsService>().To<DebugAnalyticsService>().AsSingle();
            }
            else
            {
                Container.Bind<IAnalyticsService>().To<NoOpAnalyticsService>().AsSingle();
            }

            Container.Decorate<IAnalyticsService>().With<AnalyticsContextDecorator>();

            if (useFirebase && IsVerboseSdkLoggingEnabled())
            {
                Container.Decorate<IAnalyticsService>().With<DebugAnalyticsDecorator>();
            }
        }

        private void BindCrashReportingService()
        {
            if (!IsCrashReportingEnabled())
            {
                Container.Bind<ICrashReportingService>().To<NoOpCrashReportingService>().AsSingle();
                return;
            }

            bool useFirebase = IsFirebaseEnabled();

            if (useFirebase)
            {
                Container.BindInterfacesTo<FirebaseCrashReportingService>().AsSingle();
            }
            else if (IsVerboseSdkLoggingEnabled())
            {
                Container.Bind<ICrashReportingService>().To<DebugCrashReportingService>().AsSingle();
            }
            else
            {
                Container.Bind<ICrashReportingService>().To<NoOpCrashReportingService>().AsSingle();
            }

            Container.Decorate<ICrashReportingService>().With<CrashReportingContextDecorator>();

            if (useFirebase && IsVerboseSdkLoggingEnabled())
            {
                Container.Decorate<ICrashReportingService>().With<DebugCrashReportingDecorator>();
            }
        }

        private bool IsAnalyticsEnabled()
        {
            return sdkSettings != null && sdkSettings.sdkEnabled && sdkSettings.analyticsEnabled;
        }

        private bool IsCrashReportingEnabled()
        {
            return sdkSettings != null && sdkSettings.sdkEnabled && sdkSettings.crashlyticsEnabled;
        }

        private bool IsFirebaseEnabled()
        {
            return sdkSettings != null && sdkSettings.sdkEnabled && sdkSettings.firebaseEnabled;
        }

        private bool IsVerboseSdkLoggingEnabled()
        {
            return sdkSettings != null && sdkSettings.sdkEnabled && sdkSettings.verboseSdkLogging;
        }
    }
}

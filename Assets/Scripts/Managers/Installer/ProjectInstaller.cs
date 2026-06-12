using Assets.Scripts.Audio;
using Assets.Scripts.Level;
using Assets.Scripts.Managers.Input;
using Assets.Scripts.Navigation;
using Assets.Scripts.Progress;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers.Installer
{
    public class ProjectInstaller : MonoInstaller
    {
        [Header("Levels")]
        [SerializeField] private LevelDefinitionListSo levelDefinitionListSo;

        [Header("Music")]
        [SerializeField] private MusicTrackLibrarySo musicTrackLibrary;

        public override void InstallBindings()
        {
            // Navigation
            Container.Bind<ISceneLoader>().To<UnitySceneLoader>().AsSingle();
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
        }
    }
}
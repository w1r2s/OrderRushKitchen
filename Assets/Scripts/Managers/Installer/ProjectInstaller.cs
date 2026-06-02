using Assets.Scripts.Audio;
using Assets.Scripts.Level;
using Assets.Scripts.Navigation;
using Assets.Scripts.Progress;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers.Installer
{
    public class ProjectInstaller : MonoInstaller
    {
        [Header("Levels config")]
        [SerializeField] private LevelDefinitionListSo levelDefinitionListSo;

        [Header("Music")]
        [SerializeField] private MusicTrackLibrarySo musicTrackLibrary;
        public override void InstallBindings()
        {
            Container.BindInstance(musicTrackLibrary);

            Container.Bind<ISceneLoader>().To<UnitySceneLoader>().AsSingle();
            Container.Bind<INavigationService>().To<NavigationService>().AsSingle();
            Container.Bind<ILoadingScreen>().FromComponentInHierarchy().AsSingle();

            Container.Bind<IAudioSettingsStorage>().To<PlayerPrefsAudioSettingsStorage>().AsSingle();
            Container.Bind<IAudioSettingsService>().To<AudioSettingsService>().AsSingle();

            Container.Bind<IMusicPlayer>().To<MusicPlayer>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<MusicVolumeController>().AsSingle();

            Container.Bind<IMusicService>().To<MusicService>().AsSingle();

            Container.Bind<IUserProgressStorage>().To<PlayerPrefsUserProgressStorage>().AsSingle();
            Container.BindInterfacesTo<UserProgressService>().AsSingle();

            Container.Bind<LevelDatabase>().AsSingle().WithArguments(levelDefinitionListSo.levels);
        }
    }
}
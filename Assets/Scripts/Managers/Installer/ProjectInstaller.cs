using Assets.Scripts.Audio;
using Assets.Scripts.Navigation;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers.Installer
{
    public class ProjectInstaller : MonoInstaller
    {
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
        }
    }
}
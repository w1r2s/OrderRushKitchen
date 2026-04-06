using Assets.Scripts.Managers.Game;
using Assets.Scripts.Managers.Input;
using Assets.Scripts.Managers.Sound;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers.Installer
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private RecipeListSo recipeListSo;
        [SerializeField] private AudioClipRefsSo audioClipRefsSo;
        [SerializeField] private MusicManager musicManager;

        public override void InstallBindings()
        {

            Container.BindInstance(recipeListSo);
            Container.BindInstance(audioClipRefsSo);
            Container.BindInstance(musicManager);

            Container.Bind<IGameService>().To<GameService>().AsSingle();
            Container.Bind<IDeliveryService>().To<DeliveryService>().AsSingle();
           
            Container.Bind<Player>().FromComponentInHierarchy().AsSingle();

            Container.Bind<Actions>().AsSingle();
            Container.Bind<IInputStorage>().To<PlayerPrefsInputStorage>().AsSingle();

            Container.BindInterfacesAndSelfTo<InputService>().AsSingle().NonLazy();

            Container.Bind<IAudioStorage>().To<PlayerPrefsAudioStorage>().AsSingle();
            Container.Bind<IAudioService>().To<AudioService>().AsSingle();

            Container.Bind<IMusicStorage>().To<PlayerPrefsMusicStorage>().AsSingle();
            Container.Bind<IMusicService>().To<MusicService>().AsSingle().NonLazy();

        }
    }
}

using Assets.Scripts.Managers.Game;
using Assets.Scripts.Managers.Input;
using Assets.Scripts.Managers.Sound;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers.Installer
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private ProcessRecipeListSo processRecipeListSo;
        [SerializeField] private DishRecipeListSo dishRecipeListSo;
        [SerializeField] private AudioClipRefsSo audioClipRefsSo;
        [SerializeField] private MusicManager musicManager;

        [SerializeField] private MenuItemDefinitionListSo menuDefinitionListSo;
        [SerializeField] private LevelDefinitionListSo levelDefinitionListSo;

        public override void InstallBindings()
        {

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
            Container.BindInterfacesTo<MusicInitializer>().AsSingle();

            Container.Bind<OptionsUI>().FromComponentInHierarchy().AsSingle();

            // Process recipes
            Container.Bind<RecipeDatabase>().AsSingle().WithArguments(processRecipeListSo.recipes);

            // Dish recipes
            Container.Bind<DishRecipeDatabase>().AsSingle().WithArguments(dishRecipeListSo.recipes);

            Container.Bind<MenuItemDatabase>().AsSingle().WithArguments(menuDefinitionListSo.items);

            Container.Bind<LevelDatabase>().AsSingle().WithArguments(levelDefinitionListSo.levels);
        }
    }
}

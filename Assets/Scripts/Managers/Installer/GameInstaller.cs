using Assets.Scripts.Audio;
using Assets.Scripts.Cooking;
using Assets.Scripts.Level;
using Assets.Scripts.Managers.Game;
using Assets.Scripts.Managers.Input;
using Assets.Scripts.Order;
using Assets.Scripts.Order.Runtime;
using Assets.Scripts.Runtime;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Selection;
using Assets.Scripts.Serving;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers.Installer
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Audio")]
        [SerializeField] private GameplayAudioLibrarySo audioClipsSo;


        [Header("Cooking area")]
        [SerializeField] private MenuItemDefinitionListSo menuDefinitionListSo;
        [SerializeField] private CookingProcessRecipeListSo processRecipes;
        [SerializeField] private KitchenObjectSo servedMenuItemContainerSo;

        [Header("Levels config")]
        [SerializeField] private LevelDefinitionListSo levelDefinitionListSo;


        public override void InstallBindings()
        {

            Container.BindInstance(audioClipsSo);

            Container.Bind<IGameService>().To<GameService>().AsSingle();

            Container.Bind<Player>().FromComponentInHierarchy().AsSingle();

            Container.Bind<Actions>().AsSingle();
            Container.Bind<IInputStorage>().To<PlayerPrefsInputStorage>().AsSingle();

            Container.BindInterfacesAndSelfTo<InputService>().AsSingle().NonLazy();

            Container.Bind<OptionsUI>().FromComponentInHierarchy().AsSingle();

            // Process recipes
            Container.Bind<CookingProcessRecipeResolver>().AsSingle().WithArguments(processRecipes.Recipes);

            Container.Bind<MenuItemDatabase>().AsSingle().WithArguments(menuDefinitionListSo.items);

            Container.Bind<LevelDatabase>().AsSingle().WithArguments(levelDefinitionListSo.levels);
            Container.Bind<ICurrentLevelProvider>().To<CurrentLevelProvider>().AsSingle();

            // order system
            Container.Bind<IOrderService>().To<OrderService>().AsSingle();
            Container.Bind<IOrderFlowService>().To<OrderFlowService>().AsSingle();
            Container.Bind<IOrderGenerationService>().To<OrderGenerationService>().AsSingle();
            Container.Bind<IOrderSubmissionService>().To<OrderSubmissionService>().AsSingle();
            Container.Bind<IMenuItemResolver>().To<MenuItemResolver>().AsSingle();

            Container.Bind<IServedMenuItemFactory>().To<ServedMenuItemFactory>().AsSingle().WithArguments(servedMenuItemContainerSo);

            Container.Bind<IItemSelectionService>().To<ItemSelectionService>().AsSingle();

            Container.Bind<ILevelProgressionService>().To<LevelProgressionService>().AsSingle();
            Container.BindInterfacesTo<LevelProgressionRuntime>().AsSingle();
            Container.BindInterfacesTo<LevelStartupInitializer>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelCompletionFlowService>().AsSingle();
            Container.Bind<ILevelResettable>().FromComponentsInHierarchy().AsCached();
            Container.Bind<LevelSceneResetService>().AsSingle();

            Container.Bind<PlateCompositionValidator>().AsSingle();
            Container.Bind<PlateAssemblyService>().AsSingle();

            Container.BindInterfacesTo<GameRuntime>().AsSingle();
            Container.BindInterfacesTo<OrderRuntime>().AsSingle();
            Container.BindInterfacesTo<GamePauseService>().AsSingle();
            Container.Bind<OrderDetailsUI>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<ModalPauseController>().AsSingle();
            Container.BindInterfacesTo<LevelRunPauseController>().AsSingle();

            Container.Bind<IOneShotAudioPlayer>().To<GameplayOneShotAudioPlayer>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IGameplayAudioEventService>().To<GameplayAudioEventService>().AsSingle();

            Container.BindInterfacesTo<OrderAudioController>().AsSingle();
            Container.BindInterfacesTo<GameplayMusicStarter>().AsSingle();
        }
    }
}

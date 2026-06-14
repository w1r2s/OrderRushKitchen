using OrderRushKitchen.Audio;
using OrderRushKitchen.Cooking;
using OrderRushKitchen.Game;
using OrderRushKitchen.Input;
using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Level;
using OrderRushKitchen.MainMenu;
using OrderRushKitchen.Menu;
using OrderRushKitchen.Order;
using OrderRushKitchen.PlayerControl;
using OrderRushKitchen.Selection;
using OrderRushKitchen.Serving;
using OrderRushKitchen.Settings;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Audio")]
        [SerializeField] private GameplayAudioLibrarySo audioClipsSo;

        [Header("Cooking area")]
        [SerializeField] private MenuItemDefinitionListSo menuDefinitionListSo;
        [SerializeField] private CookingProcessRecipeListSo processRecipes;
        [SerializeField] private KitchenObjectSo servedMenuItemContainerSo;

        public override void InstallBindings()
        {
            BindConfiguration();
            BindGameplay();
            BindCookingAndServing();
            BindOrders();
            BindSelection();
            BindLevelFlow();
            BindUI();
            BindAudio();
        }

        private void BindConfiguration()
        {
            Container.BindInstance(audioClipsSo);
            Container.Bind<MenuItemDatabase>().AsSingle().WithArguments(menuDefinitionListSo.items);
            Container.Bind<CookingProcessRecipeResolver>().AsSingle().WithArguments(processRecipes.Recipes);
        }

        private void BindGameplay()
        {
            Container.Bind<IGameService>().To<GameService>().AsSingle();
            Container.BindInterfacesTo<GamePauseService>().AsSingle();
            Container.Bind<Player>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<InputService>().AsSingle();
            Container.BindInterfacesTo<GameRuntime>().AsSingle();
        }

        private void BindCookingAndServing()
        {
            Container.Bind<PlateCompositionValidator>().AsSingle();
            Container.Bind<PlateAssemblyService>().AsSingle();
            Container.Bind<PlateServingService>().AsSingle();
            Container.Bind<IServedMenuItemFactory>().To<ServedMenuItemFactory>().AsSingle().WithArguments(servedMenuItemContainerSo);
        }

        private void BindOrders()
        {
            Container.Bind<IOrderService>().To<OrderService>().AsSingle();
            Container.Bind<IOrderFlowService>().To<OrderFlowService>().AsSingle();
            Container.Bind<IOrderGenerationService>().To<OrderGenerationService>().AsSingle();
            Container.Bind<IOrderSubmissionService>().To<OrderSubmissionService>().AsSingle();
            Container.Bind<IMenuItemResolver>().To<MenuItemResolver>().AsSingle();
            Container.BindInterfacesTo<OrderRuntime>().AsSingle();
            Container.BindInterfacesTo<OrderAudioController>().AsSingle();
        }

        private void BindSelection()
        {
            Container.Bind<IItemSelectionService>().To<ItemSelectionService>().AsSingle();
            Container.Bind<ItemSelectionUI>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<ItemSelectionUIController>().AsSingle();
        }

        private void BindLevelFlow()
        {
            Container.Bind<ICurrentLevelProvider>().To<CurrentLevelProvider>().AsSingle();
            Container.Bind<ILevelProgressionService>().To<LevelProgressionService>().AsSingle();
            Container.BindInterfacesTo<LevelProgressionRuntime>().AsSingle();
            Container.BindInterfacesTo<LevelStartupInitializer>().AsSingle();
            Container.BindInterfacesTo<LevelCompletionFlowService>().AsSingle();
            Container.Bind<ILevelResettable>().FromComponentsInHierarchy().AsCached();
            Container.Bind<LevelSceneResetService>().AsSingle();
            Container.BindInterfacesTo<LevelRunPauseController>().AsSingle();
            Container.BindInterfacesTo<LevelCompletionNavigationController>().AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<OptionsUI>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<OptionsUIController>().AsSingle();
            Container.Bind<OrdersPanelUI>().FromComponentInHierarchy().AsSingle();
            Container.Bind<OrderDetailsUI>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<OrdersPanelUIController>().AsSingle();
            Container.Bind<GameOverUI>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GamePauseUI>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<ModalPauseController>().AsSingle();
            Container.BindInterfacesTo<GameOptionsPauseController>().AsSingle();
            Container.BindInterfacesTo<GamePauseActionsController>().AsSingle();
            Container.BindInterfacesTo<GameOverNavigationController>().AsSingle();
        }

        private void BindAudio()
        {
            Container.Bind<IOneShotAudioPlayer>().To<GameplayOneShotAudioPlayer>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IGameplayAudioEventService>().To<GameplayAudioEventService>().AsSingle();
            Container.BindInterfacesTo<GameplayMusicStarter>().AsSingle();
        }
    }
}
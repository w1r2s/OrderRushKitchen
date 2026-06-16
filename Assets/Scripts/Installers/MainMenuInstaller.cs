using OrderRushKitchen.Audio;
using OrderRushKitchen.Game;
using OrderRushKitchen.MainMenu;
using OrderRushKitchen.Settings;
using Zenject;

namespace OrderRushKitchen.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MainMenuMusicStarter>().AsSingle();

            Container.Bind<OptionsUI>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<OptionsUIController>().AsSingle();
        }
    }
}
using Assets.Scripts.Audio;
using Assets.Scripts.UI;
using Zenject;

namespace Assets.Scripts.Managers.Installer
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
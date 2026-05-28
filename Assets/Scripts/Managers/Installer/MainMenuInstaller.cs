using Assets.Scripts.Audio;
using Zenject;

namespace Assets.Scripts.Managers.Installer
{
    public class MainMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MainMenuMusicStarter>().AsSingle();
        }
    }
}
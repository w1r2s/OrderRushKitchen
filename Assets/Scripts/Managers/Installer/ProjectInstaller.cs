using Assets.Scripts.Navigation;
using Zenject;

namespace Assets.Scripts.Managers.Installer
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISceneLoader>().To<UnitySceneLoader>().AsSingle();
            Container.Bind<INavigationService>().To<NavigationService>().AsSingle();
            Container.Bind<ILoadingScreen>().FromComponentInHierarchy().AsSingle();
        }
    }
}
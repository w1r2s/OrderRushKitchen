using Cysharp.Threading.Tasks;
using System.Threading;
using Zenject;

namespace Assets.Scripts.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingScreen _loadingScreen;
        private bool _isLoading;

        [Inject]
        public NavigationService(ISceneLoader sceneLoader, ILoadingScreen loadingScreen)
        {
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
        }

        public UniTask LoadGameAsync(CancellationToken cancellationToken)
        {
            return LoadAsync(GameSceneId.Game, cancellationToken);
        }

        public UniTask LoadMainMenuAsync(CancellationToken cancellationToken)
        {
            return LoadAsync(GameSceneId.MainMenu, cancellationToken);
        }

        public UniTask ReloadGameAsync(CancellationToken cancellationToken)
        {
            return LoadAsync(GameSceneId.Game, cancellationToken);
        }

        private async UniTask LoadAsync(GameSceneId sceneId, CancellationToken cancellationToken)
        {
            if (_isLoading)
                return;

            _isLoading = true;

            _loadingScreen.Show();
            _loadingScreen.SetProgress(0f);
            try
            {
                await _sceneLoader.LoadAsync(sceneId, cancellationToken);
                _loadingScreen.SetProgress(1f);
            }
            finally
            {
                _loadingScreen.Hide();
                _isLoading = false;
            }
        }
    }
}
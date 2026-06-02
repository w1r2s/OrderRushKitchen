using Assets.Scripts.Navigation;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Level
{
    public class LevelCompletionNavigationController : IInitializable, IDisposable
    {
        private readonly ILevelCompletionFlowService _levelCompletionService;
        private readonly INavigationService _navigationService;

        [Inject]
        public LevelCompletionNavigationController(ILevelCompletionFlowService levelCompletionService, INavigationService navigationService)
        {
            _levelCompletionService = levelCompletionService;
            _navigationService = navigationService;
        }

        public void Initialize()
        {
            _levelCompletionService.OnReturnToMenuRequested += LevelCompletionService_OnReturnToMenuRequested;
        }
        public void Dispose()
        {
            _levelCompletionService.OnReturnToMenuRequested -= LevelCompletionService_OnReturnToMenuRequested;
        }

        private void LevelCompletionService_OnReturnToMenuRequested(object sender, EventArgs e)
        {
            _navigationService.LoadMainMenuAsync(CancellationToken.None).Forget(HandleLoadException);
        }

        private static void HandleLoadException(Exception ex)
        {
            if (ex is OperationCanceledException)
                return;

            Debug.LogException(ex);
        }
    }
}

using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.SceneManagement;

namespace OrderRushKitchen.Navigation
{
    public class UnitySceneLoader : ISceneLoader
    {
        public async UniTask LoadAsync(GameSceneId sceneId, CancellationToken cancellationToken)
        {
            var sceneName = ResolveSceneName(sceneId);
            await SceneManager.LoadSceneAsync(sceneName).ToUniTask(cancellationToken: cancellationToken);
        }
        private static string ResolveSceneName(GameSceneId sceneId)
        {
            return sceneId switch
            {
                GameSceneId.MainMenu => "MainMenuScene",
                GameSceneId.Game => "GameScene",
                _ => throw new System.ArgumentOutOfRangeException(nameof(sceneId), sceneId, null)
            };
        }
    }
}
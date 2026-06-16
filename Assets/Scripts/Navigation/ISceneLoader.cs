using Cysharp.Threading.Tasks;
using System.Threading;

namespace OrderRushKitchen.Navigation
{
    public interface ISceneLoader
    {
        UniTask LoadAsync(GameSceneId sceneId, CancellationToken cancellationToken);
    }
}
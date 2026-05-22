using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.Navigation
{
    public interface ISceneLoader
    {
        UniTask LoadAsync(GameSceneId sceneId, CancellationToken cancellationToken);
    }
}
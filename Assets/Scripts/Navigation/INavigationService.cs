using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.Navigation
{
    public interface INavigationService
    {
        UniTask LoadMainMenuAsync(CancellationToken cancellationToken);
        UniTask LoadGameAsync(CancellationToken cancellationToken);
        UniTask ReloadGameAsync(CancellationToken cancellationToken);
    }
}

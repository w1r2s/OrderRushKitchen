using Cysharp.Threading.Tasks;
using System.Threading;

namespace OrderRushKitchen.Navigation
{
    public interface INavigationService
    {
        UniTask LoadMainMenuAsync(CancellationToken cancellationToken);
        UniTask LoadGameAsync(CancellationToken cancellationToken);
        UniTask ReloadGameAsync(CancellationToken cancellationToken);
    }
}

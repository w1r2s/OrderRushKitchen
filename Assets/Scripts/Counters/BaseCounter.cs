using OrderRushKitchen.Audio;
using OrderRushKitchen.Interaction;
using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.PlayerControl;
using Zenject;

namespace OrderRushKitchen.Counters
{

    public abstract class BaseCounter : ObjectHolder, IPlayerInteractable
    {
        private IGameplayAudioEventService _audioService;

        [Inject]
        protected void ConstructBase(IGameplayAudioEventService audioService)
        {
            _audioService = audioService;
        }

        public abstract void Interact(Player player);
        public virtual bool TryInteractAlternate(Player player)
        {
            return false;
        }

        protected bool TryPlaceObjectFromPlayer(Player player)
        {
            if (!player.TryTransferObjectTo(this))
                return false;

            _audioService.Play(GameplayAudioEvent.DropGeneric, transform.position);
            return true;
        }
    }
}

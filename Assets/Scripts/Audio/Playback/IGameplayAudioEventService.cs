using UnityEngine;

namespace OrderRushKitchen.Audio
{
    public interface IGameplayAudioEventService
    {
        void Play(GameplayAudioEvent audioEvent, Vector3 position, float volumeMultiplier = 1f);
        void PlayGlobal(GameplayAudioEvent audioEvent, float volumeMultiplier = 1f);
    }
}

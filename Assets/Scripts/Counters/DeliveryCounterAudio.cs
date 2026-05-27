using Assets.Scripts.Audio;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Counters
{
    public class DeliveryCounterAudio : MonoBehaviour
    {
        private IGameplayAudioEventService _gameplayAudio;
        [SerializeField] private DeliveryCounter counter;

        [Inject]
        private void Construct(IGameplayAudioEventService gameplayAudio)
        {
            _gameplayAudio = gameplayAudio;
        }

        private void Start()
        {
            counter.OnDeliverySuccess += Counter_OnDeliverySuccess;
            counter.OnDeliveryFail += Counter_OnDeliveryFail;
        }

        private void OnDestroy()
        {
            if (counter != null)
            {
                counter.OnDeliverySuccess -= Counter_OnDeliverySuccess;
                counter.OnDeliveryFail -= Counter_OnDeliveryFail;
            }
        }

        private void Counter_OnDeliverySuccess(object sender, EventArgs e)
        {
            _gameplayAudio.Play(GameplayAudioEvent.DishSubmittedSuccess, counter.transform.position);
        }

        private void Counter_OnDeliveryFail(object sender, EventArgs e)
        {
            _gameplayAudio.Play(GameplayAudioEvent.DishSubmittedFail, counter.transform.position);
        }
    }
}
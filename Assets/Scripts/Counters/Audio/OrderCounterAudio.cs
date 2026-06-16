using OrderRushKitchen.Audio;
using System;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Counters
{
    public class OrderCounterAudio : MonoBehaviour
    {
        private IGameplayAudioEventService _gameplayAudio;
        [SerializeField] private OrderCounter counter;

        [Inject]
        private void Construct(IGameplayAudioEventService gameplayAudio)
        {
            _gameplayAudio = gameplayAudio;
        }

        private void Start()
        {
            counter.OnOrderAccepted += Counter_OnOrderAccepted;
            counter.OnOrderAcceptFailed += Counter_OnOrderAcceptFailed;
        }

        private void OnDestroy()
        {
            if (counter != null)
            {
                counter.OnOrderAccepted -= Counter_OnOrderAccepted;
                counter.OnOrderAcceptFailed -= Counter_OnOrderAcceptFailed;
            }
        }

        private void Counter_OnOrderAccepted(object sender, EventArgs e)
        {
            _gameplayAudio.Play(GameplayAudioEvent.OrderWriteAccepted, counter.transform.position);
        }

        private void Counter_OnOrderAcceptFailed(object sender, OrderCounterOrderFailedEventArgs e)
        {
            _gameplayAudio.Play(GameplayAudioEvent.OrderRejected, counter.transform.position);
        }
    }
}
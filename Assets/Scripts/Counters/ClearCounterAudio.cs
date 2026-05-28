using Assets.Scripts.Audio;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Counters
{
    public class ClearCounterAudio : MonoBehaviour
    {
        private IGameplayAudioEventService _gameplayAudio;
        [SerializeField] private ClearCounter counter;

        [Inject]
        private void Construct(IGameplayAudioEventService gameplayAudio)
        {
            _gameplayAudio = gameplayAudio;
        }

        private void Start()
        {
            counter.OnInvalidAction += Counter_OnInvalidAction;
        }
        private void OnDestroy()
        {
            if (counter != null)
            {
                counter.OnInvalidAction -= Counter_OnInvalidAction;
            }
        }

        private void Counter_OnInvalidAction(object sender, EventArgs e)
        {
            _gameplayAudio.Play(GameplayAudioEvent.InvalidAction, counter.transform.position);
        }
    }
}

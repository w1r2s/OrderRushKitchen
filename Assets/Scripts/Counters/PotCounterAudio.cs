using Assets.Scripts.Audio;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Counters
{
    public class PotCounterAudio : MonoBehaviour
    {
        [SerializeField] private PotCounter potCounter;
        [SerializeField] private GameplayAudioLoopPlayer loopPlayer;

        private IGameplayAudioEventService _audioEventService;

        [Inject]
        private void Construct(IGameplayAudioEventService audioEventService)
        {
            _audioEventService = audioEventService;
        }

        private void Start()
        {
            if (potCounter == null || loopPlayer == null)
            {
                Debug.LogError($"{nameof(PotCounterAudio)} is not configured.", this);
                enabled = false;
                return;
            }

            potCounter.OnStateChanged += PotCounter_OnStateChanged;
            potCounter.OnIngredientAdded += PotCounter_OnIngredientAdded;
            potCounter.OnCleared += PotCounter_OnCleared;
        }

        private void OnDestroy()
        {
            if (potCounter == null)
                return;

            potCounter.OnStateChanged -= PotCounter_OnStateChanged;
            potCounter.OnIngredientAdded -= PotCounter_OnIngredientAdded;
            potCounter.OnCleared -= PotCounter_OnCleared;
        }

        private void PotCounter_OnStateChanged(object sender, PotCounter.OnStateChangedEventArgs e)
        {
            var shouldPlayLoop = e.State == PotCounter.State.Cooking;
            loopPlayer.SetPlaying(shouldPlayLoop);

            if (e.State == PotCounter.State.Cooked)
            {
                _audioEventService.Play(GameplayAudioEvent.PlateServed, potCounter.transform.position);
            }
        }
        private void PotCounter_OnIngredientAdded(object sender, EventArgs e)
        {
            _audioEventService.Play(GameplayAudioEvent.DropGeneric, potCounter.transform.position);
        }

        private void PotCounter_OnCleared(object sender, EventArgs e)
        {
            _audioEventService.Play(GameplayAudioEvent.TrashItem, potCounter.transform.position);
        }
    }
}
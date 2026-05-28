using Assets.Scripts.Audio;
using Assets.Scripts.Managers.Game;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Counters
{
    public class StoveCounterAudio : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;
        [SerializeField] private GameplayAudioLoopPlayer loopPlayer;

        [SerializeField, Range(0f, 1f)] private float burnWarningProgress = 0.5f;
        [SerializeField, Min(0.01f)] private float warningInterval = 0.2f;

        private bool playWarningSound;
        private float warningSoundTimer;

        private IGameplayAudioEventService _audioEventService;
        private IGameClock _gameClock;

        [Inject]
        private void Construct(IGameplayAudioEventService audioEventService, IGameClock gameClock)
        {
            _audioEventService = audioEventService;
            _gameClock = gameClock;
        }

        private void Start()
        {
            stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
            stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
            stoveCounter.OnInvalidAction += StoveCounter_OnInvalidAction;
        }

        private void OnDestroy()
        {
            if (stoveCounter == null)
                return;

            stoveCounter.OnStateChanged -= StoveCounter_OnStateChanged;
            stoveCounter.OnProgressChanged -= StoveCounter_OnProgressChanged;
            stoveCounter.OnInvalidAction -= StoveCounter_OnInvalidAction;
        }

        private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
        {
            playWarningSound = stoveCounter.IsFried() && e.progressNormalized >= burnWarningProgress;
        }

        private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
        {
            bool shouldPlayLoop = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
            loopPlayer.SetPlaying(shouldPlayLoop);

            if (!shouldPlayLoop)
            {
                playWarningSound = false;
                warningSoundTimer = 0f;
            }
        }
        private void StoveCounter_OnInvalidAction(object sender, System.EventArgs e)
        {
            _audioEventService.Play(GameplayAudioEvent.InvalidAction, stoveCounter.transform.position);
        }

        private void Update()
        {
            if (!playWarningSound)
                return;

            warningSoundTimer -= _gameClock.DeltaTime;
            if (warningSoundTimer > 0f)
                return;

            warningSoundTimer = warningInterval;
            _audioEventService.Play(GameplayAudioEvent.BurnWarning, stoveCounter.transform.position);
        }
    }
}
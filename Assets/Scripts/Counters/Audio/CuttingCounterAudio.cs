using OrderRushKitchen.Audio;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Counters
{
    public class CuttingCounterAudio : MonoBehaviour
    {
        private IGameplayAudioEventService _audioService;
        [SerializeField] private CuttingCounter counter;

        [Inject]
        private void Construct(IGameplayAudioEventService audioService)
        {
            _audioService = audioService;
        }

        private void Start()
        {
            counter.OnCut += Counter_OnCut;
            counter.OnInvalidAction += Counter_OnInvalidAction;
        }
        private void OnDestroy()
        {
            if (counter != null)
            {
                counter.OnCut -= Counter_OnCut;
                counter.OnInvalidAction -= Counter_OnInvalidAction;
            }
        }

        private void Counter_OnInvalidAction(object sender, System.EventArgs e)
        {
            _audioService.Play(GameplayAudioEvent.InvalidAction, counter.transform.position);
        }

        private void Counter_OnCut(object sender, System.EventArgs e)
        {
            _audioService.Play(GameplayAudioEvent.CuttingAction, counter.transform.position);
        }
    }
}
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers.Sound
{
    public class CounterSoundConnector : MonoBehaviour
    {
        [SerializeField] private BaseCounter[] counters;

        private IAudioService _audio;

        [Inject]
        private void Construct(IAudioService audio)
        {
            _audio = audio;
        }

        private void Start()
        {
            foreach (var counter in counters)
            {
                counter.OnObjectPlaced += OnObjectPlaced;
            }
        }

        private void OnDestroy()
        {
            foreach (var counter in counters)
            {
                counter.OnObjectPlaced -= OnObjectPlaced;
            }
        }

        private void OnObjectPlaced(object sender, EventArgs e)
        {
            var counter = (BaseCounter)sender;
            var volume = 1f;
            _audio.PlayDrop(counter.transform.position, volume);
        }
    }
}

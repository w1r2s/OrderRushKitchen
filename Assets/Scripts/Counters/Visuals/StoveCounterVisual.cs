using UnityEngine;

namespace OrderRushKitchen.Counters
{

    public class StoveCounterVisual : MonoBehaviour
    {

        [SerializeField] private ParticleSystem fryingParticles;

        [SerializeField] private StoveCounter stoveCounter;

        private void Start()
        {
            fryingParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        }

        private void OnDestroy()
        {
            if (stoveCounter != null)
                stoveCounter.OnStateChanged -= StoveCounter_OnStateChanged;
        }
        private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
        {
            bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;

            if (showVisual && !fryingParticles.isPlaying)
            {
                fryingParticles.Play();
            }
            if (!showVisual && fryingParticles.isPlaying)
            {
                fryingParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }
}

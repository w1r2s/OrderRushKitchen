using Assets.Scripts.Audio;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Serving
{
    public class PlateKitchenObjectAudio : MonoBehaviour
    {
        [SerializeField] private PlateKitchenObject plate;
        private IGameplayAudioEventService _gameplayAudio;

        [Inject]
        private void Construct(IGameplayAudioEventService gameplayAudio)
        {
            _gameplayAudio = gameplayAudio;
        }

        private void Start()
        {
            plate.OnIngredientsChanged += Plate_OnIngredientsChanged;
            plate.OnPlateStateChanged += Plate_OnPlateStateChanged;
        }
        private void OnDestroy()
        {
            if (plate != null)
            {
                plate.OnIngredientsChanged -= Plate_OnIngredientsChanged;
                plate.OnPlateStateChanged -= Plate_OnPlateStateChanged;
            }
        }

        private void Plate_OnPlateStateChanged(object sender, PlateStateChangedEventArgs e)
        {
            if (e.PlateState != PlateState.Served)
                return;

            _gameplayAudio.Play(GameplayAudioEvent.PlateServed, plate.transform.position);
        }

        private void Plate_OnIngredientsChanged(object sender, EventArgs e)
        {
            _gameplayAudio.Play(GameplayAudioEvent.DropGeneric, plate.transform.position);
        }
    }
}
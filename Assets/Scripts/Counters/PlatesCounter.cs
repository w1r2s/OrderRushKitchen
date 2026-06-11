using Assets.Scripts.Managers.Game;
using System;
using UnityEngine;
using Zenject;

public class PlatesCounter : BaseCounter
{
    private IGameClock _clock;
    private IGameService _gameService;
    public event EventHandler onPlateSpawned;
    public event EventHandler onPlateRemoved;

    [SerializeField] KitchenObjectSo plateKitchenObjectSo;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;

    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    [Inject]
    private void Construct(IGameService gameService, IGameClock clock)
    {
        _gameService = gameService;
        _clock = clock;
    }
    private void Update()
    {
        spawnPlateTimer += _clock.DeltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;

            if (_gameService.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;
                onPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public override void Interact(Player player)
    {
        if (!player.HasObject)
        {
            if (platesSpawnedAmount > 0)
            {
                if (player.TrySpawnAndSet(plateKitchenObjectSo.prefab, out _))
                {
                    platesSpawnedAmount--;
                    onPlateRemoved?.Invoke(this, EventArgs.Empty);
                }
            }
        }

    }
    public override void ResetForLevelTransition()
    {
        base.ResetForLevelTransition();

        for (int i = platesSpawnedAmount; i > 0; i--)
        {
            platesSpawnedAmount--;
            onPlateRemoved?.Invoke(this, EventArgs.Empty);
        }
        spawnPlateTimer = 0f;
    }
}

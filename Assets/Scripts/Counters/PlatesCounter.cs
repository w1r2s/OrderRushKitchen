using Assets.Scripts;
using Assets.Scripts.Managers.Game;
using System;
using UnityEngine;
using Zenject;

public class PlatesCounter : BaseCounter
{
    private IGameService _gameService;
    public event EventHandler onPlateSpawned;
    public event EventHandler onPlateRemoved;

    [SerializeField] KitchenObjectSo plateKitchenObjectSo;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;

    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    [Inject]
    private void Construct(IGameService gameService)
    {
        _gameService = gameService;
    }
    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if(spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;

            if(_gameService.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;
                onPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject())
        {
            if(platesSpawnedAmount > 0)
            {
                platesSpawnedAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSo, player);

                onPlateRemoved?.Invoke(this,EventArgs.Empty);
            }
        }

    }
}

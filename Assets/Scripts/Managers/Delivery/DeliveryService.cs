using Assets.Scripts.Managers.Game;
using Assets.Scripts.Managers.Sound;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    public class DeliveryService : IDeliveryService
    {
        public event EventHandler OnRecipeSpawned;
        public event EventHandler OnRecipeCompleted;

        public event EventHandler OnRecipeSuccess;
        public event EventHandler OnRecipeFailed;

        private readonly RecipeListSo _recipeListSo;
        private readonly IGameService _gameService;
        private readonly IAudioService _audioService;

        private List<RecipeSo> _waitingRecipes;

        private float spawnTimer;
        private float spawnTimerMax = 4f;
        private int waitingRecipeMax = 4;
        private int successfulRecipes;
        private float volume = 1f;

        public DeliveryService(RecipeListSo recipeListSo, IGameService gameService, IAudioService audioService)
        {
            _gameService = gameService;
            _recipeListSo = recipeListSo;
            _waitingRecipes = new List<RecipeSo>();
            _audioService = audioService;
        }
        public void Tick(float deltaTime)
        {
            spawnTimer += deltaTime;

            if (spawnTimer > spawnTimerMax)
            {
                spawnTimer = 0;

                if (_gameService.IsGamePlaying() && _waitingRecipes.Count < waitingRecipeMax)
                {
                    RecipeSo recipe = _recipeListSo.recipeSoList[UnityEngine.Random.Range(0, _recipeListSo.recipeSoList.Count)];
                    _waitingRecipes.Add(recipe);
                    OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
                }
            }
        }


        public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
        {
            for (int i = 0; i < _waitingRecipes.Count; i++)
            {
                RecipeSo waitingRecipeSo = _waitingRecipes[i];
                if (waitingRecipeSo.kitchenObjectSoList.Count == plateKitchenObject.GetKitchenObjectSoList().Count)
                {
                    bool plateMatch = true;
                    foreach (KitchenObjectSo recipeKitchenObjectSo in waitingRecipeSo.kitchenObjectSoList)
                    {
                        bool found = false;
                        foreach (KitchenObjectSo plateKitchenObjectSo in plateKitchenObject.GetKitchenObjectSoList())
                        {
                            if (plateKitchenObjectSo == recipeKitchenObjectSo)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            plateMatch = false;
                        }
                    }
                    if (plateMatch)
                    {
                        successfulRecipes++;

                        _waitingRecipes.RemoveAt(i);
                        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                        OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                      
                        _audioService.PlayRecipeSuccess(plateKitchenObject.transform.position, volume);
                        return;
                    }
                }
            }
            OnRecipeFailed?.Invoke(this, EventArgs.Empty);
            _audioService.PlayRecipeFail(plateKitchenObject.transform.position, volume);
        }

        public List<RecipeSo> GetWaitingRecipes()
        {
            return _waitingRecipes;
        }

        public int GetSuccessfulRecipesAmount()
        {
            return successfulRecipes;
        }
    }
}

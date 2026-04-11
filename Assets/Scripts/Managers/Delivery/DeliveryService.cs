using Assets.Scripts.Managers.Game;
using Assets.Scripts.Managers.Sound;
using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class DeliveryService : IDeliveryService
    {
        public event EventHandler OnRecipeSpawned;
        public event EventHandler OnRecipeCompleted;

        public event EventHandler OnRecipeSuccess;
        public event EventHandler OnRecipeFailed;

        private readonly DishRecipeDatabase _dishRecipes;
        private readonly IGameService _gameService;
        private readonly IAudioService _audioService;

        private List<DishRecipeSo> _waitingRecipes = new();

        private float spawnTimer;
        private float spawnTimerMax = 4f;
        private int waitingRecipeMax = 4;
        private int successfulRecipes;

        public DeliveryService(DishRecipeDatabase dishRecipes, IGameService gameService, IAudioService audioService)
        {
            _gameService = gameService;
            _dishRecipes = dishRecipes;
            _waitingRecipes = new List<DishRecipeSo>();
            _audioService = audioService;
        }
        public void Tick(float deltaTime)
        {
            if (!_gameService.IsGamePlaying())
                return;

            spawnTimer += deltaTime;

            if (spawnTimer < spawnTimerMax)
                return;

            spawnTimer = 0;

            if (_waitingRecipes.Count >= waitingRecipeMax)
                return;

            var recipe = _dishRecipes.GetRandomRecipe();
            _waitingRecipes.Add(recipe);
            OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
        }


        public void DeliverRecipe(PlateKitchenObject plate)
        {
            var plateIngredients = plate.GetKitchenObjectSoList();
            for (int i = 0; i < _waitingRecipes.Count; i++)
            {
                var recipe = _waitingRecipes[i];
                if (IsMatch(recipe, plateIngredients))
                {
                    successfulRecipes++;
                    _waitingRecipes.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);

                    _audioService.PlayRecipeSuccess(plate.transform.position);
                    return;
                }
            }
            OnRecipeFailed?.Invoke(this, EventArgs.Empty);
            _audioService.PlayRecipeFail(plate.transform.position);
        }
        private bool IsMatch(DishRecipeSo recipe, IReadOnlyList<KitchenObjectSo> ingredients)
        {
            if (recipe.ingredients.Count != ingredients.Count)
                return false;

            foreach (var ingredient in recipe.ingredients)
            {
                if (!ingredients.Contains(ingredient))
                    return false;
            }

            return true;
        }
        public List<DishRecipeSo> GetWaitingRecipes()
        {
            return _waitingRecipes;
        }

        public int GetSuccessfulRecipesAmount()
        {
            return successfulRecipes;
        }
    }
}

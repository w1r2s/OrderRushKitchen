using OrderRushKitchen.Cooking;
using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.PlayerControl;
using OrderRushKitchen.Serving;
using System;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Counters
{

    public class CuttingCounter : BaseCounter, IHasProgress
    {
        private CookingProcessRecipeResolver _recipesResolver;
        private PlateAssemblyService _plateAssemblyService;

        private int cuttingProgress;
        private ActionCookingProcessRecipeSo cutRecipe;

        public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
        public event EventHandler OnCut;
        public event EventHandler OnInvalidAction;

        [Inject]
        private void Construct(
            CookingProcessRecipeResolver recipesResolver,
            PlateAssemblyService plateAssemblyService)
        {
            _recipesResolver = recipesResolver;
            _plateAssemblyService = plateAssemblyService;
        }

        public override void Interact(Player player)
        {
            if (!HasObject)
            {
                TryPlaceCuttableObject(player);
                return;
            }

            if (!player.HasObject)
            {
                if (TryTransferObjectTo(player))
                {
                    ResetCuttingState();
                }

                return;
            }

            TryAddObjectToPlate(player);
        }

        public override bool TryInteractAlternate(Player player)
        {
            if (!HasObject || cutRecipe == null)
                return false;

            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);

            if (cutRecipe.RequiredActions > 0)
            {
                EmitProgress((float)cuttingProgress / cutRecipe.RequiredActions);
            }

            if (cuttingProgress >= cutRecipe.RequiredActions)
            {
                CompleteCut(cutRecipe);
            }

            return true;
        }

        public override void ResetForLevelTransition()
        {
            base.ResetForLevelTransition();
            ResetCuttingState();
        }

        private void TryPlaceCuttableObject(Player player)
        {
            if (!player.HasObject)
                return;

            KitchenObject playerObject = player.GetObject();

            if (!_recipesResolver.TryGetSingleInputRecipe<ActionCookingProcessRecipeSo>(CookingProcessType.Cutting, playerObject.KitchenObjectSo, out var recipe))
            {
                OnInvalidAction?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (!TryPlaceObjectFromPlayer(player))
                return;

            cuttingProgress = 0;
            cutRecipe = recipe;
            EmitProgress(0f);
        }

        private void TryAddObjectToPlate(Player player)
        {
            if (!player.TryGetObjectAs<PlateKitchenObject>(out var plate))
                return;

            if (!_plateAssemblyService.TryAddIngredientFrom(plate, this))
            {
                OnInvalidAction?.Invoke(this, EventArgs.Empty);
                return;
            }

            ResetCuttingState();
        }

        private void CompleteCut(ActionCookingProcessRecipeSo recipe)
        {
            if (recipe == null ||
                recipe.OutputKitchenObject == null ||
                recipe.OutputKitchenObject.prefab == null)
            {
                if (TryRemoveAndDestroyObject())
                {
                    ResetCuttingState();
                }

                return;
            }

            if (!TryRemoveAndDestroyObject())
                return;

            if (!TrySpawnAndSet(recipe.OutputKitchenObject.prefab, out _))
            {
                ResetCuttingState();
                return;
            }

            ResetCuttingState();
        }

        private void ResetCuttingState()
        {
            cuttingProgress = 0;
            cutRecipe = null;
            EmitProgress(0f);
        }

        private void EmitProgress(float progressNormalized)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = Mathf.Clamp01(progressNormalized)
            });
        }
    }

}

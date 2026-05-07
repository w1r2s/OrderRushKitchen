using Assets.Scripts;
using Assets.Scripts.Managers.Sound;
using Assets.Scripts.Serving;
using System;
using UnityEngine;
using Zenject;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private CuttingRecipeSo[] cuttingRecipes;
    private IAudioService _audioService;
    private RecipeDatabase _recipes;
    private PlateAssemblyService _plateAssemblyService;

    private int cuttingProgress;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [Inject]
    private void Construct(IAudioService audioService, RecipeDatabase recipes, PlateAssemblyService plateAssemblyService)
    {
        _audioService = audioService;
        _recipes = recipes;
        _plateAssemblyService = plateAssemblyService;

    }
    public override void Interact(Player player)
    {
        // попробовать положить на стол
        if (!HasObject)
        {
            if (!player.HasObject)
                return;

            var playerObject = player.GetObject();

            if (!_recipes.TryGetRecipe<CuttingRecipeSo>(RecipeType.Cutting, playerObject.KitchenObjectSo, out var recipe))
                return;

            PlaceObjectFromPlayer(player);

            cuttingProgress = 0;
            UpdateProgress(recipe);
            return;

        }
        // забрать объект со стола
        if (!player.HasObject)
        {
            var obj = RemoveObject();
            player.SetObject(obj);
            return;
        }
        // попробовать положить на тарелку
        var counterObj = GetObject();

        if (player.TryGetObjectAs<PlateKitchenObject>(out var plate))
        {
            if (!_plateAssemblyService.TryAddIngredient(plate, counterObj.KitchenObjectSo))
                return;

            RemoveAndDestroy();
        }

    }
    public override void InteractAlternate(Player player)
    {
        if (!HasObject)
            return;
        var counterObj = GetObject();

        if (!_recipes.TryGetRecipe<CuttingRecipeSo>(RecipeType.Cutting, counterObj.KitchenObjectSo, out var recipe))
            return;

        cuttingProgress++;
        OnCut?.Invoke(this, EventArgs.Empty);
        _audioService.PlayCut(transform.position);
        UpdateProgress(recipe);

        if (cuttingProgress >= recipe.cuttingProgressMax)
        {
            CompleteCut(recipe);
        }
    }

    private void UpdateProgress(CuttingRecipeSo recipe)
    {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = (float)cuttingProgress / recipe.cuttingProgressMax
        });
    }
    private void RemoveAndDestroy()
    {
        var obj = RemoveObject();
        if (obj != null)
        {
            Destroy(obj.gameObject);
        }
    }
    private void CompleteCut(CuttingRecipeSo recipe)
    {
        RemoveAndDestroy();
        KitchenObject newObj = Instantiate(recipe.output.prefab);
        SetObject(newObj);
    }
}

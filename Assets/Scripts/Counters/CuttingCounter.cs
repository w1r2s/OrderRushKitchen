using Assets.Scripts.Cooking;
using Assets.Scripts.Managers.Sound;
using Assets.Scripts.Serving;
using System;
using Zenject;

public class CuttingCounter : BaseCounter, IHasProgress
{
    private IAudioService _audioService;
    private CookingProcessRecipeResolver _recipesResolver;
    private PlateAssemblyService _plateAssemblyService;

    private int cuttingProgress;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    private ActionCookingProcessRecipeSo cutRecipe;

    [Inject]
    private void Construct(IAudioService audioService, CookingProcessRecipeResolver recipesResolver, PlateAssemblyService plateAssemblyService)
    {
        _audioService = audioService;
        _recipesResolver = recipesResolver;
        _plateAssemblyService = plateAssemblyService;

    }
    public override void Interact(Player player)
    {
        if (!HasObject)
        {
            if (!player.HasObject)
                return;

            var playerObject = player.GetObject();

            if (!_recipesResolver.TryGetSingleInputRecipe<ActionCookingProcessRecipeSo>(CookingProcessType.Cutting, playerObject.KitchenObjectSo, out var recipe))
                return;

            PlaceObjectFromPlayer(player);

            cuttingProgress = 0;
            cutRecipe = recipe;
            UpdateProgress(cutRecipe);
            return;

        }

        if (!player.HasObject)
        {
            var obj = RemoveObject();
            player.SetObject(obj);
            ResetCuttingState();
            return;
        }

        var counterObj = GetObject();

        if (player.TryGetObjectAs<PlateKitchenObject>(out var plate))
        {
            if (_plateAssemblyService.TryAddIngredient(plate, counterObj.KitchenObjectSo))
            {
                RemoveAndDestroy();
                ResetCuttingState();
            }
        }

    }
    public override void InteractAlternate(Player player)
    {
        if (!HasObject)
            return;

        if (cutRecipe == null)
            return;

        cuttingProgress++;
        OnCut?.Invoke(this, EventArgs.Empty);
        _audioService.PlayCut(transform.position);
        UpdateProgress(cutRecipe);

        if (cuttingProgress >= cutRecipe.RequiredActions)
        {
            CompleteCut(cutRecipe);
        }
    }

    private void UpdateProgress(ActionCookingProcessRecipeSo recipe)
    {
        if (recipe == null || recipe.RequiredActions <= 0)
            return;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = (float)cuttingProgress / recipe.RequiredActions
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
    private void CompleteCut(ActionCookingProcessRecipeSo recipe)
    {
        RemoveAndDestroy();

        KitchenObject newObj = Instantiate(recipe.OutputKitchenObject.prefab);
        SetObject(newObj);

        ResetCuttingState();
    }

    private void ResetCuttingState()
    {
        cuttingProgress = 0;
        cutRecipe = null;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = 0f
        });
    }

    public override void ResetForLevelTransition()
    {
        base.ResetForLevelTransition();
        ResetCuttingState();
    }

}

using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private CuttingRecipeSo[] cuttingRecipeSoArray;

    public event EventHandler <IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler onCut;

    private int cuttingProgress;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSo()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeSo cuttingRecipeSo = GetCuttingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSo());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSo.cuttingProgressMax,
                    }); 
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            {
                if (player.GetKitchenObject().TryGetPlate(out var plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSo()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
        }

    }
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSo()))
        {
            cuttingProgress++;

            onCut?.Invoke(this,EventArgs.Empty);

            CuttingRecipeSo cuttingRecipeSo = GetCuttingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSo());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSo.cuttingProgressMax,
            });

            if (cuttingProgress >= cuttingRecipeSo.cuttingProgressMax)
            {
                KitchenObjectSo outputKitchenObjectSo = GetOutputForInput(GetKitchenObject().GetKitchenObjectSo());
                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSo, this);
            }
        }
    }
    private KitchenObjectSo GetOutputForInput(KitchenObjectSo kitchenObjectSo)
    {
        CuttingRecipeSo cuttingRecipeSo = GetCuttingRecipeSoWithInput(kitchenObjectSo);
        if (cuttingRecipeSo != null)
        {
            return cuttingRecipeSo.output;
        }
        return null;
    }
    private bool HasRecipeWithInput(KitchenObjectSo kitchenObjectSo)
    {
        CuttingRecipeSo cuttingRecipeSo = GetCuttingRecipeSoWithInput(kitchenObjectSo);
        return cuttingRecipeSo != null;
    }
    private CuttingRecipeSo GetCuttingRecipeSoWithInput(KitchenObjectSo kitchenObjectSo)
    {
        foreach (var recipeSo in cuttingRecipeSoArray)
        {
            if (recipeSo.input == kitchenObjectSo)
            {
                return recipeSo;
            }
        }
        return null;
    }
}

using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSo[] cuttingRecipeSoArray;

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
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }

    }
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSo()))
        {
            cuttingProgress++;
            CuttingRecipeSo cuttingRecipeSo = GetCuttingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSo());
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

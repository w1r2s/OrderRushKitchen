using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeSo : ProcessRecipeSo
{
    public int cuttingProgressMax;
    public override RecipeType Type => RecipeType.Cutting;
}

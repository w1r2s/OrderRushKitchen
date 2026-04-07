using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSo : ProcessRecipeSo
{
    public float fryingTimerMax;
    public override RecipeType Type => RecipeType.Frying;
}

using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeSo : ProcessRecipeSo
{
    public float burningTimerMax;
    public override RecipeType Type => RecipeType.Burning;
}

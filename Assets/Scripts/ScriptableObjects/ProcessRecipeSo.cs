using UnityEngine;

public abstract class ProcessRecipeSo : ScriptableObject
{
    public KitchenObjectSo input;
    public KitchenObjectSo output;
    public abstract RecipeType Type { get; }
}
public enum RecipeType
{
    Cutting,
    Frying,
    Burning
}

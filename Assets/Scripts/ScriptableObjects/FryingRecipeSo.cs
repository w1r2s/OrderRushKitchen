using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSo : ScriptableObject
{
    public KitchenObjectSo input;
    public KitchenObjectSo output;
    public float fryingTimerMax;
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSo : ScriptableObject
{
    public List<KitchenObjectSo> kitchenObjectSoList;
    public string recipeName;
}

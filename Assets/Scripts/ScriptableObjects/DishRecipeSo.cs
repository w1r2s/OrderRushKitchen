using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu()]
    public class DishRecipeSo : ScriptableObject
    {
        public string recipeName;
        public List<KitchenObjectSo> ingredients;
    }
}

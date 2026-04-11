using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu()]
    public class DishRecipeListSo : ScriptableObject
    {
        public List<DishRecipeSo> recipes;
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Cooking
{
    [CreateAssetMenu(menuName = "KitchenChaos/Cooking/Process Recipe List")]
    public class CookingProcessRecipeListSo : ScriptableObject
    {
        [SerializeField] private List<CookingProcessRecipeSo> cookingProcessRecipes;
        public List<CookingProcessRecipeSo> Recipes => cookingProcessRecipes;
    }
}

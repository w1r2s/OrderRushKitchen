using System.Collections.Generic;
using UnityEngine;

namespace OrderRushKitchen.Cooking
{
    [CreateAssetMenu(menuName = "Order Rush Kitchen/Cooking/Process Recipe List")]
    public class CookingProcessRecipeListSo : ScriptableObject
    {
        [SerializeField] private List<CookingProcessRecipeSo> cookingProcessRecipes;
        public List<CookingProcessRecipeSo> Recipes => cookingProcessRecipes;
    }
}

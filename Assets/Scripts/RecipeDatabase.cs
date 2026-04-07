using System.Collections.Generic;

namespace Assets.Scripts
{
    public class RecipeDatabase
    {
        private Dictionary<RecipeType, Dictionary<KitchenObjectSo, ProcessRecipeSo>> _recipes;

        public RecipeDatabase(List<ProcessRecipeSo> recipes)
        {
            _recipes = new();

            foreach (var recipe in recipes)
            {
                if (!_recipes.TryGetValue(recipe.Type, out var dict))
                {
                    dict = new Dictionary<KitchenObjectSo, ProcessRecipeSo>();
                    _recipes[recipe.Type] = dict;
                }
                dict[recipe.input] = recipe;
            }
        }

        public bool TryGetRecipe<T>(RecipeType type, KitchenObjectSo input, out T recipe) where T : ProcessRecipeSo
        {
            recipe = null;
            if (!_recipes.TryGetValue(type, out var dict))
                return false;

            if (!dict.TryGetValue(input, out var baseRecipe))
                return false;

            recipe = baseRecipe as T;
            return recipe != null;
        }
    }
}

using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;

public class DishRecipeDatabase
{
    private readonly List<DishRecipeSo> _recipes;

    public DishRecipeDatabase(List<DishRecipeSo> recipes)
    {
        _recipes = recipes;
    }

    public DishRecipeSo GetMatchingRecipe(List<KitchenObjectSo> ingredients)
    {
        foreach (var recipe in _recipes)
        {
            if (IsMatch(recipe, ingredients))
                return recipe;
        }

        return null;
    }

    private bool IsMatch(DishRecipeSo recipe, List<KitchenObjectSo> ingredients)
    {
        if (recipe.ingredients.Count != ingredients.Count)
            return false;

        foreach (var ingredient in recipe.ingredients)
        {
            if (!ingredients.Contains(ingredient))
                return false;
        }

        return true;
    }
}
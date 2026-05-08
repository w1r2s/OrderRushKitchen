using Assets.Scripts.Composition;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Cooking
{
    public class CookingProcessRecipeResolver
    {
        private readonly Dictionary<CookingProcessType, List<CookingProcessRecipeSo>> _recipesByType;

        public CookingProcessRecipeResolver(List<CookingProcessRecipeSo> recipes)
        {
            if (recipes == null)
                throw new ArgumentNullException(nameof(recipes));

            _recipesByType = new();

            foreach (var recipe in recipes)
            {
                if (recipe == null)
                    continue;

                if (_recipesByType.TryGetValue(recipe.Type, out var value))
                {
                    value.Add(recipe);
                }
                else
                {
                    var listByType = new List<CookingProcessRecipeSo>() { recipe };
                    _recipesByType[recipe.Type] = listByType;
                }
            }
        }

        public bool CanAddInput(CookingProcessType processType, KitchenObjectSo candidate, IReadOnlyList<KitchenObjectSo> currentInputs)
        {
            if (candidate == null)
                return false;

            var counts = IngredientComposition.BuildCounts(currentInputs, candidate);

            if (counts.Count == 0)
                return false;

            if (!_recipesByType.TryGetValue(processType, out var recipesByType))
                return false;

            foreach (var recipe in recipesByType)
            {
                var recipeCounts = IngredientComposition.BuildCounts(recipe.Inputs);

                if (IngredientComposition.IsSubsetOf(counts, recipeCounts))
                    return true;
            }

            return false;
        }

        public bool TryGetSingleInputRecipe(CookingProcessType processType, KitchenObjectSo input, out CookingProcessRecipeSo recipe)
        {
            recipe = null;

            if (input == null)
                return false;

            var inputList = new List<KitchenObjectSo>() { input };

            return TryResolveExact(processType, inputList, out recipe);
        }

        public bool TryResolveExact<T>(CookingProcessType type, IReadOnlyList<KitchenObjectSo> inputs, out T recipe) where T : CookingProcessRecipeSo
        {
            recipe = null;

            if (inputs == null || inputs.Count == 0)
                return false;

            if (!_recipesByType.TryGetValue(type, out var recipesByType))
                return false;

            var inputCounts = IngredientComposition.BuildCounts(inputs);

            foreach (var item in recipesByType)
            {
                var recipeCounts = IngredientComposition.BuildCounts(item.Inputs);

                if (IngredientComposition.AreEqual(inputCounts, recipeCounts))
                {
                    recipe = item as T;
                    return recipe != null;
                }
            }

            return false;
        }
        public bool TryGetSingleInputRecipe<T>(CookingProcessType processType, KitchenObjectSo input, out T recipe) where T : CookingProcessRecipeSo
        {
            recipe = null;

            if (!TryGetSingleInputRecipe(processType, input, out var baseRecipe))
                return false;

            recipe = baseRecipe as T;
            return recipe != null;
        }

    }
}

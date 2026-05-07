using Assets.Scripts.Level;
using System.Collections.Generic;
using Zenject;

namespace Assets.Scripts.Serving
{
    public class PlateCompositionValidator
    {
        private readonly ICurrentLevelProvider _levelProvider;
        private readonly MenuItemDatabase _menuDatabase;

        [Inject]
        public PlateCompositionValidator(ICurrentLevelProvider levelProvider, MenuItemDatabase menuDatabase)
        {
            _levelProvider = levelProvider;
            _menuDatabase = menuDatabase;
        }

        public bool CanAddIngredient(KitchenObjectSo candidate, IReadOnlyList<KitchenObjectSo> currentIngredients)
        {
            if (candidate == null || currentIngredients == null)
                return false;

            var level = _levelProvider.CurrentLevel.levelNumber;
            var menuItems = _menuDatabase.GetAvailableForLevel(level);
            if (menuItems.Count == 0)
                return false;


            var candidateDict = BuildCounts(currentIngredients, candidate);


            foreach (var menuItem in menuItems)
            {
                if (menuItem == null || menuItem.requiredIngredients == null)
                    continue;

                var requiredIngredients = menuItem.requiredIngredients;

                if (currentIngredients.Count + 1 > requiredIngredients.Count)
                    continue;

                var dict = BuildCounts(requiredIngredients);

                bool found = true;
                foreach (var key in candidateDict.Keys)
                {
                    if (!dict.TryGetValue(key, out var value))
                    {
                        found = false;
                        break;
                    }

                    if (candidateDict[key] > value)
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                    return true;

            }

            return false;
        }

        private Dictionary<KitchenObjectSo, int> BuildCounts(IReadOnlyList<KitchenObjectSo> ingredients, KitchenObjectSo candidate = null)
        {

            Dictionary<KitchenObjectSo, int> result = new();

            foreach (KitchenObjectSo ingredient in ingredients)
            {
                if (!result.TryAdd(ingredient, 1))
                {
                    result[ingredient]++;
                }
            }

            if (candidate != null)
            {
                if (!result.TryAdd(candidate, 1))
                {
                    result[candidate]++;
                }
            }

            return result;
        }
    }
}

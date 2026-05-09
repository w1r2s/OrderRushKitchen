using Assets.Scripts.Composition;
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


            var candidateCounts = IngredientComposition.BuildCounts(currentIngredients, candidate);

            foreach (var menuItem in menuItems)
            {
                if (menuItem == null || menuItem.requiredIngredients == null)
                    continue;

                var requiredIngredients = menuItem.requiredIngredients;

                if (currentIngredients.Count + 1 > requiredIngredients.Count)
                    continue;

                var requiredCounts = IngredientComposition.BuildCounts(requiredIngredients);

                if (IngredientComposition.IsSubsetOf(candidateCounts, requiredCounts))
                    return true;

            }

            return false;
        }

    }
}

using OrderRushKitchen.KitchenObjects;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Serving
{
    public class PlateAssemblyService
    {
        private readonly PlateCompositionValidator _plateValidator;


        [Inject]
        public PlateAssemblyService(PlateCompositionValidator plateValidator)
        {
            _plateValidator = plateValidator;
        }

        public bool TryAddIngredient(PlateKitchenObject plate, KitchenObjectSo candidate)
        {
            if (plate == null)
                return false;

            if (!_plateValidator.CanAddIngredient(candidate, plate.Ingredients))
                return false;

            if (!plate.TryAddIngredient(candidate))
                return false;

            return true;
        }

        public bool TryAddIngredientFrom(PlateKitchenObject plate, ObjectHolder ingredientHolder)
        {
            if (plate == null || ingredientHolder == null)
                return false;

            KitchenObject ingredient = ingredientHolder.GetObject();

            if (ingredient == null || ReferenceEquals(ingredient, plate))
                return false;

            if (!TryAddIngredient(plate, ingredient.KitchenObjectSo))
                return false;

            if (!ingredientHolder.TryRemoveAndDestroyObject())
            {
                Debug.LogError(
                    $"{ingredientHolder.name}: ingredient was added to plate, " +
                    "but could not be removed from holder");
            }

            return true;
        }
    }
}

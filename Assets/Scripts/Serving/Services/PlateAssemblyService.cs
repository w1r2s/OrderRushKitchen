using Zenject;

namespace Assets.Scripts.Serving
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
    }
}

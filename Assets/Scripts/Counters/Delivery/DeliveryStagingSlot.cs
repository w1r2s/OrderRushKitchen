using OrderRushKitchen.Composition;
using OrderRushKitchen.KitchenObjects;
using UnityEngine;

namespace OrderRushKitchen.Counters
{
    public sealed class DeliveryStagingSlot : ObjectHolder
    {
        [Range(0.1f, 1f)]
        [SerializeField] private float stagedScale = 0.625f;

        public bool TryApplyStagedPresentation()
        {
            KitchenObject kitchenObject = GetObject();

            if (kitchenObject == null)
                return false;

            kitchenObject.transform.localScale *= stagedScale;

            IngredientsIconsUI[] ingredientsIcons = kitchenObject.GetComponentsInChildren<IngredientsIconsUI>(true);

            foreach (IngredientsIconsUI ingredientsIconsUI in ingredientsIcons)
            {
                ingredientsIconsUI.Hide();
            }

            return true;
        }
    }
}
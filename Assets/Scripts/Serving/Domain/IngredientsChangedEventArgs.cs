using System;
using System.Collections.Generic;

namespace Assets.Scripts.Serving
{
    public class IngredientsChangedEventArgs : EventArgs
    {
        public IReadOnlyList<KitchenObjectSo> IngredientsOnPlate { get; }

        public IngredientsChangedEventArgs(IReadOnlyList<KitchenObjectSo> ingredientsOnPlate)
        {
            IngredientsOnPlate = ingredientsOnPlate;
        }
    }
}

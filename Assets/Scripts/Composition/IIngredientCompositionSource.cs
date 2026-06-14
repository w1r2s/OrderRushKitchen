using OrderRushKitchen.KitchenObjects;
using System.Collections.Generic;
using System;

namespace OrderRushKitchen.Composition
{
    public interface IIngredientCompositionSource
    {
        event EventHandler OnIngredientsChanged;
        IReadOnlyList<KitchenObjectSo> Ingredients { get; }
    }
}

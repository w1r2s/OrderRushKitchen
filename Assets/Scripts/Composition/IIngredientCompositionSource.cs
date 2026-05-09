using System;
using System.Collections.Generic;

namespace Assets.Scripts.Composition
{
    public interface IIngredientCompositionSource
    {
        event EventHandler OnIngredientsChanged;
        IReadOnlyList<KitchenObjectSo> Ingredients { get; }
    }
}

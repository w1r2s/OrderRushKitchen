using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Level;
using OrderRushKitchen.Menu;
using System.Collections.Generic;
using UnityEngine;

namespace OrderRushKitchen.Cooking
{
    public abstract class CookingProcessRecipeSo : ScriptableObject
    {
        [Header("Type")]
        [SerializeField] private CookingProcessType type;

        [Header("Input")]
        [SerializeField] private List<KitchenObjectSo> inputs;

        [Header("Output")]
        [SerializeField] private KitchenObjectSo outputKitchenObject;
        [SerializeField] private MenuItemDefinitionSo outputMenuItem;

        public CookingProcessType Type => type;
        public IReadOnlyList<KitchenObjectSo> Inputs => inputs;
        public KitchenObjectSo OutputKitchenObject => outputKitchenObject;
        public MenuItemDefinitionSo OutputMenuItem => outputMenuItem;

    }
}

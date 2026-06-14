using OrderRushKitchen.KitchenObjects;
using System.Collections.Generic;
using UnityEngine;

namespace OrderRushKitchen.Menu
{
    [CreateAssetMenu(menuName = "Order Rush Kitchen/Menu Item Definition")]
    public class MenuItemDefinitionSo : ScriptableObject
    {
        [Header("Identity")]
        public string key;
        public string displayName;

        [Header("Type")]
        public MenuItemCategory category;

        [Header("Presentation")]
        public Sprite icon;
        public GameObject servedVisualPrefab;

        [Header("Gameplay")]
        [Min(1)]
        public int difficulty = 1;

        [Min(0.1f)]
        public float preparationTime = 30f;

        [Min(0f)]
        public float baseSpawnWeight = 1f;

        [Min(1)]
        public int minLevel = 1;

        [Header("Dish Composition")]
        public List<KitchenObjectSo> requiredIngredients = new();
    }
}

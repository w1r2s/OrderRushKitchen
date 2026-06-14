using System.Collections.Generic;
using UnityEngine;

namespace OrderRushKitchen.Menu
{
    [CreateAssetMenu(menuName = "Order Rush Kitchen/Menu Item Definition List")]
    public class MenuItemDefinitionListSo : ScriptableObject
    {
        public List<MenuItemDefinitionSo> items = new();
    }
}

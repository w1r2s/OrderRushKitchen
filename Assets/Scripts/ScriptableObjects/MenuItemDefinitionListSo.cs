using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "KitchenChaos/Menu Item Definition List")]
    public class MenuItemDefinitionListSo : ScriptableObject
    {
        public List<MenuItemDefinitionSo> items = new();
    }
}

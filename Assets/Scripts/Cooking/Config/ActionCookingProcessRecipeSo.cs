using UnityEngine;

namespace OrderRushKitchen.Cooking
{
    [CreateAssetMenu(menuName = "Order Rush Kitchen/Cooking/Action Process Recipe")]
    public class ActionCookingProcessRecipeSo : CookingProcessRecipeSo
    {
        [Header("Gameplay")]
        [Min(1)]
        [SerializeField] private int requiredActions;

        public int RequiredActions => requiredActions;
    }
}

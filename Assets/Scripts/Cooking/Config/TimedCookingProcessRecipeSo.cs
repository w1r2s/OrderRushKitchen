using UnityEngine;

namespace OrderRushKitchen.Cooking
{
    [CreateAssetMenu(menuName = "Order Rush Kitchen/Cooking/Timed Process Recipe")]
    public class TimedCookingProcessRecipeSo : CookingProcessRecipeSo
    {
        [Header("Gameplay")]
        [Min(1)]
        [SerializeField] private float duration;

        public float Duration => duration;
    }
}

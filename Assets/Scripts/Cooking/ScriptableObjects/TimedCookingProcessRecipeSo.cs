using UnityEngine;

namespace Assets.Scripts.Cooking
{
    [CreateAssetMenu(menuName = "KitchenChaos/Cooking/Timed Process Recipe")]
    public class TimedCookingProcessRecipeSo : CookingProcessRecipeSo
    {
        [Header("Gameplay")]
        [Min(1)]
        [SerializeField] private float duration;

        public float Duration => duration;
    }
}

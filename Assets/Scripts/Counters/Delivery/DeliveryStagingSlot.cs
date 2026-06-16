using OrderRushKitchen.Composition;
using OrderRushKitchen.KitchenObjects;
using UnityEngine;

namespace OrderRushKitchen.Counters
{
    public sealed class DeliveryStagingSlot : ObjectHolder
    {
        [Range(0.1f, 1f)]
        [SerializeField] private float stagedScale = 0.625f;
        [SerializeField, Min(0.1f)] private float resolveDuration = 1f;

        private Vector3 _resolveStartScale;
        private float _resolveElapsed;
        private bool _isResolving;

        public bool IsResolving => _isResolving;

        public bool TryApplyStagedPresentation()
        {
            KitchenObject kitchenObject = GetObject();

            if (kitchenObject == null)
                return false;

            kitchenObject.transform.localScale *= stagedScale;

            IngredientsIconsUI[] ingredientsIcons = kitchenObject.GetComponentsInChildren<IngredientsIconsUI>(true);

            foreach (IngredientsIconsUI ingredientsIconsUI in ingredientsIcons)
            {
                ingredientsIconsUI.Hide();
            }

            return true;
        }

        public bool TryBeginResolvePresentation()
        {
            KitchenObject kitchenObject = GetObject();

            if (kitchenObject == null || _isResolving)
                return false;

            _resolveStartScale = kitchenObject.transform.localScale;
            _resolveElapsed = 0f;
            _isResolving = true;
            return true;
        }

        public bool TickResolvePresentation(float deltaTime)
        {
            if (!_isResolving)
                return false;

            KitchenObject kitchenObject = GetObject();

            if (kitchenObject == null)
            {
                _isResolving = false;
                return true;
            }

            _resolveElapsed += Mathf.Max(0f, deltaTime);

            float duration = Mathf.Max(0.01f, resolveDuration);
            float progress = Mathf.Clamp01(_resolveElapsed / duration);
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);

            kitchenObject.transform.localScale = Vector3.LerpUnclamped(_resolveStartScale, Vector3.zero, easedProgress);

            if (progress < 1f)
                return false;

            _isResolving = false;
            return true;
        }
    }
}
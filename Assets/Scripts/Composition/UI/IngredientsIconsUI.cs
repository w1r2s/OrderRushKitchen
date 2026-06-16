using OrderRushKitchen.KitchenObjects;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace OrderRushKitchen.Composition
{
    public class IngredientsIconsUI : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour compositionSourceBehaviour;
        [SerializeField] private Transform iconTemplate;

        private IIngredientCompositionSource _compositionSource;

        private void Awake()
        {
            if (iconTemplate != null)
                iconTemplate.gameObject.SetActive(false);

            _compositionSource = compositionSourceBehaviour as IIngredientCompositionSource;
            if (_compositionSource == null)
            {
                Debug.LogError($"{name}: compositionSourceBehaviour must implement {nameof(IIngredientCompositionSource)}");
                enabled = false;
            }
        }

        private void Start()
        {
            if (_compositionSource == null || iconTemplate == null)
                return;

            _compositionSource.OnIngredientsChanged += CompositionSource_OnIngredientsChanged;
            UpdateVisual(_compositionSource.Ingredients);
        }

        private void OnDestroy()
        {
            if (_compositionSource != null)
                _compositionSource.OnIngredientsChanged -= CompositionSource_OnIngredientsChanged;
        }

        private void CompositionSource_OnIngredientsChanged(object sender, EventArgs e)
        {
            UpdateVisual(_compositionSource.Ingredients);
        }

        private void UpdateVisual(IReadOnlyList<KitchenObjectSo> ingredients)
        {
            ClearIcons();

            if (ingredients == null)
                return;

            foreach (var kitchenObjectSo in ingredients)
            {
                if (kitchenObjectSo == null)
                    continue;

                var iconTransform = Instantiate(iconTemplate, transform);
                iconTransform.gameObject.SetActive(true);

                if (iconTransform.TryGetComponent<IngredientIconUI>(out var iconUI))
                {
                    iconUI.SetKitchenObjectSo(kitchenObjectSo);
                }
            }
        }

        private void ClearIcons()
        {
            foreach (Transform child in transform)
            {
                if (child == iconTemplate)
                    continue;

                Destroy(child.gameObject);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
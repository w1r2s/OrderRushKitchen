using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Serving
{
    public class PlateContentVisual : MonoBehaviour
    {
        [Serializable]
        public struct IngredientVisualEntry
        {
            public KitchenObjectSo ingredient;
            public GameObject prefab;
            [Min(0f)]
            public float stackHeight;
        }

        [Serializable]
        public struct MenuItemVisualEntry
        {
            public MenuItemDefinitionSo menuItem;
            public GameObject prefab;
        }

        [SerializeField] private PlateKitchenObject plate;
        [SerializeField] private Transform contentRoot;
        [SerializeField] private float initialShiftY = 0.5f;
        [SerializeField] private List<IngredientVisualEntry> ingredientVisualEntries;
        [SerializeField] private List<MenuItemVisualEntry> menuItemVisualEntries;


        private List<GameObject> spawnedVisuals;

        private void Awake()
        {
            spawnedVisuals = new List<GameObject>();
        }
        private void Start()
        {
            plate.OnIngredientsChanged += Plate_OnIngredientsChanged;
            plate.OnPlateStateChanged += Plate_OnPlateStateChanged;

            Initialize();
        }
        private void OnDestroy()
        {
            plate.OnIngredientsChanged -= Plate_OnIngredientsChanged;
            plate.OnPlateStateChanged -= Plate_OnPlateStateChanged;
        }
        private void Initialize()
        {
            ClearSpawnedVisuals();

            if (plate.State == PlateState.Assembly)
            {

                if (plate.Ingredients.Count == 0)
                    return;

                RenderAssembly(plate.Ingredients);
            }
            else
            {
                var resolvedMenuItem = plate.ResolvedMenuItem;
                if (resolvedMenuItem == null)
                    return;

                foreach (var menuItem in menuItemVisualEntries)
                {
                    if (menuItem.menuItem == resolvedMenuItem)
                    {
                        var obj = Instantiate(menuItem.prefab, contentRoot, false);
                        spawnedVisuals.Add(obj);
                        break;
                    }
                }

            }
        }
        private void Plate_OnIngredientsChanged(object sender, EventArgs e)
        {
            if (plate.State != PlateState.Assembly)
                return;
            ClearSpawnedVisuals();

            RenderAssembly(plate.Ingredients);

        }
        private void Plate_OnPlateStateChanged(object sender, PlateStateChangedEventArgs e)
        {
            if (e.PlateState != PlateState.Served || e.ResolvedMenuItem == null)
                return;

            ClearSpawnedVisuals();
            foreach (var menuItem in menuItemVisualEntries)
            {
                if (menuItem.menuItem == e.ResolvedMenuItem)
                {
                    var obj = Instantiate(menuItem.prefab, contentRoot, false);
                    spawnedVisuals.Add(obj);
                    break;
                }
            }
            if (spawnedVisuals.Count == 0)
            {
                Debug.LogWarning($"menuItemVisualEntries missing for menu item: {e.ResolvedMenuItem?.key}");
            }

        }
        private void RenderAssembly(IReadOnlyList<KitchenObjectSo> ingredients)
        {
            var currentY = initialShiftY;
            foreach (var kObj in ingredients)
            {
                foreach (var ingredientEntry in ingredientVisualEntries)
                {
                    if (ingredientEntry.ingredient == kObj)
                    {
                        var newObj = Instantiate(ingredientEntry.prefab, contentRoot, false);
                        newObj.transform.localPosition = new Vector3(0, currentY, 0);
                        currentY += ingredientEntry.stackHeight;

                        spawnedVisuals.Add(newObj);
                        break;
                    }
                }
            }
        }
        private void ClearSpawnedVisuals()
        {
            if (spawnedVisuals != null)
            {
                foreach (var obj in spawnedVisuals)
                {
                    Destroy(obj);
                }
                spawnedVisuals.Clear();
            }
        }
    }
}
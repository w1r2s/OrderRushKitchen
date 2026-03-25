using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSo_GameObject
    {
        public KitchenObjectSo kitchenObjectSo;
        public GameObject kitchenObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSo_GameObject> kitchenObjectSo_GameObjectList;
    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        foreach (var kitchenObjectSoGameObject in kitchenObjectSo_GameObjectList)
        {
            kitchenObjectSoGameObject.kitchenObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (var kitchenObjectSoGameObject in kitchenObjectSo_GameObjectList)
        {
            if (kitchenObjectSoGameObject.kitchenObjectSo == e.kitchenObjectSo)
            {
                kitchenObjectSoGameObject.kitchenObject.SetActive(true);
            }
        }
    }
}

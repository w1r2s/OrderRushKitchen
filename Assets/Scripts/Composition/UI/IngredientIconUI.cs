using OrderRushKitchen.KitchenObjects;
using UnityEngine.UI;
using UnityEngine;

namespace OrderRushKitchen.Composition
{
    public class IngredientIconUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        public void SetKitchenObjectSo(KitchenObjectSo kitchenObjectSo)
        {
            image.sprite = kitchenObjectSo.sprite;
        }
    }
}
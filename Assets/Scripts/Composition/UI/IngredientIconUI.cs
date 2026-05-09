using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Composition
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
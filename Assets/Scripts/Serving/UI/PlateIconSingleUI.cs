using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Serving
{
    public class PlateIconSingleUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        public void SetKitchenObjectSo(KitchenObjectSo kitchenObjectSo)
        {
            image.sprite = kitchenObjectSo.sprite;
        }
    }
}
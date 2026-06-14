using UnityEngine.UI;
using UnityEngine;

namespace OrderRushKitchen.Order
{
    public class OrderItemSectionUI : MonoBehaviour
    {
        [SerializeField] private Image dishIconImage;

        [SerializeField] private CanvasGroup sectionCanvasGroup;

        public void Bind(OrderItem item)
        {
            Refresh(item);
        }

        public void Refresh(OrderItem item)
        {
            if (item == null || item.MenuItem == null)
                return;

            if (dishIconImage != null && item.MenuItem.icon != null)
            {
                dishIconImage.sprite = item.MenuItem.icon;
            }

            if (sectionCanvasGroup != null)
            {
                sectionCanvasGroup.alpha = item.IsCompleted ? 0.65f : 1f;
            }
        }
    }
}

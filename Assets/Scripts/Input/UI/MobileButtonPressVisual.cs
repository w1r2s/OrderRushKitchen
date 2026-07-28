using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OrderRushKitchen.Input
{
    public class MobileButtonPressVisual : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private Image _image;

        [SerializeField, Range(0.1f, 1f)]
        private float pressedScaleMultiplier = 0.92f;

        [SerializeField]
        private Color pressedColorMultiplier = new(0.8f, 0.8f, 0.8f, 1f);

        private Color _normalColor;
        private Vector3 _normalScale;

        private void Awake()
        {
            _normalColor = _image.color;
            _normalScale = transform.localScale;

        }
        private void OnDisable()
        {
            RestoreNormalState();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            _image.color = _normalColor * pressedColorMultiplier;
            transform.localScale = _normalScale * pressedScaleMultiplier;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            RestoreNormalState();
        }

        private void RestoreNormalState()
        {
            if (_image == null)
                return;

            _image.color = _normalColor;
            transform.localScale = _normalScale;
        }
    }
}

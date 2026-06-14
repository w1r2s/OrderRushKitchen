using OrderRushKitchen.Counters;
using System;
using UnityEngine.UI;
using UnityEngine;

namespace OrderRushKitchen.Order
{
    public class OrderCounterResultUI : MonoBehaviour
    {
        [SerializeField] private Image resultImage;
        [SerializeField] private OrderCounter orderCounter;
        [SerializeField] private Sprite successSprite;
        [SerializeField] private Sprite failedSprite;

        [SerializeField, Min(0.1f)] private float activeTime = 1.25f;
        private float currentActive = 0f;

        private void Start()
        {
            if (resultImage == null || orderCounter == null)
                return;
            orderCounter.OnOrderAccepted += OrderCounter_OnOrderAccepted;
            orderCounter.OnOrderAcceptFailed += OrderCounter_OnOrderAcceptFailed;
            Hide();
        }
        private void OnDestroy()
        {
            if (resultImage == null || orderCounter == null)
                return;
            orderCounter.OnOrderAccepted -= OrderCounter_OnOrderAccepted;
            orderCounter.OnOrderAcceptFailed -= OrderCounter_OnOrderAcceptFailed;
        }
        private void Update()
        {
            if (resultImage == null) 
                return;

            if (resultImage.gameObject.activeSelf)
            {
                if (currentActive >= activeTime)
                {
                    Hide();
                    return;
                }
                currentActive += Time.deltaTime;
            }
        }
        private void OrderCounter_OnOrderAccepted(object sender, EventArgs e)
        {

            resultImage.sprite = successSprite;
            Show();
        }
        private void OrderCounter_OnOrderAcceptFailed(object sender, OrderCounterOrderFailedEventArgs e)
        {
            resultImage.sprite = failedSprite;
            Show();
        }
        private void Show()
        {
            resultImage.gameObject.SetActive(true);
            currentActive = 0f;
        }
        private void Hide()
        {
            resultImage?.gameObject.SetActive(false);
            currentActive = 0f;
        }
    }
}

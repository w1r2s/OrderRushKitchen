using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Order
{
    public class OrderRuntimeProbe : MonoBehaviour
    {
        private IOrderService _orderService;

        [Inject]
        private void Construct(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private void Start()
        {
            _orderService.OnOrderCreated += OrderService_OnOrderCreated;
            _orderService.OnOrderUpdated += OrderService_OnOrderUpdated;
            _orderService.OnOrderFailed += OrderService_OnOrderFailed;
            _orderService.OnOrderRemoved += OrderService_OnOrderRemoved;
            _orderService.OnOrderCompleted += OrderService_OnOrderCompleted;
        }



        private void OnDestroy()
        {
            _orderService.OnOrderCreated -= OrderService_OnOrderCreated;
            _orderService.OnOrderUpdated -= OrderService_OnOrderUpdated;
            _orderService.OnOrderFailed -= OrderService_OnOrderFailed;
            _orderService.OnOrderRemoved -= OrderService_OnOrderRemoved;
            _orderService.OnOrderCompleted -= OrderService_OnOrderCompleted;
        }

        private void OrderService_OnOrderCreated(object sender, OrderServiceEventArgs e)
        {
            var order = e.Order;
            List<string> menuNames = order.OrderItems.Select(item => item.MenuItem.name).ToList();
            var outMessage = $"Order created event:" + $"order id: {order.Id}\n";
            if (menuNames.Count > 0)
            {
                for (int i = 0; i < menuNames.Count; i++)
                {
                    outMessage += $"order item: {menuNames[i]}\n";
                }
            }
            outMessage += $"order max time: {order.MaxTime}";

            Debug.Log(outMessage);
        }
        private void OrderService_OnOrderUpdated(object sender, OrderServiceEventArgs e)
        {
            var order = e.Order;
            Debug.Log($"Order {order.Id} updated!\n" +
                $"Order Status: {order.OrderStatus}\n" +
                $"Completed items: {order.CompletedItems}\n" +
                $"remaining time: {order.RemainingTime}\n" +
                $"progress: {order.ProgressNormalized}");
        }
        private void OrderService_OnOrderFailed(object sender, OrderServiceEventArgs e)
        {
            var order = e.Order;
            Debug.Log($"Order {order.Id} failed.\n" +
                $"Completed items: {order.CompletedItems} \n" +
                $"progress: {order.ProgressNormalized}");
        }
        private void OrderService_OnOrderCompleted(object sender, OrderServiceEventArgs e)
        {
            var order = e.Order;
            Debug.Log($"Order {order.Id} completed.\n" +
               $"Completed items: {order.CompletedItems}\n" +
               $"progress: {order.ProgressNormalized}");
        }

        private void OrderService_OnOrderRemoved(object sender, OrderServiceEventArgs e)
        {
            var order = e.Order;
            Debug.Log($"Order {order.Id} removed.\n" +
                $"Order Status: {order.OrderStatus}\n" +
                $"Completed items: {order.CompletedItems}\n" +
                $"remaining time: {order.RemainingTime}\n" +
                $"progress: {order.ProgressNormalized}");
        }
    }
}

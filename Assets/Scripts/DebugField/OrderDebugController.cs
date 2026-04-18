using Assets.Scripts.Order;
using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.DebugField
{
    public class OrderDebugController : MonoBehaviour
    {
        private IOrderService _orderService;

        [SerializeField] List<MenuItemDefinitionSo> menuItems;
        [SerializeField] MenuItemDefinitionSo outputMenuItem;

        [Inject]
        private void Construct(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                CreateOrder(menuItems);
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                TryFulfillOrder(outputMenuItem);
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                GetActiveOrders();
            }
        }

        private void CreateOrder(IEnumerable<MenuItemDefinitionSo> menuItems)
        {
            var order = _orderService.CreateOrder(menuItems);
            if (order == null)
            {
                Debug.Log("crete order: order is null");
            }
            else
            {
                Debug.Log($"order id: {order.Id},\n " +
                      $"order items: {order.OrderItems.Count},\n " +
                      $"order max time: {order.MaxTime},\n " +
                      $"order status: {order.OrderStatus},\n " +
                      $"order remaining time: {order.RemainingTime}");
            }
        }
        private void TryFulfillOrder(MenuItemDefinitionSo deliveredItem)
        {
            if (deliveredItem == null)
            {
                Debug.Log("delivered item is null");
                return;
            }
            if (_orderService.TryFulfillOrderItem(deliveredItem))
            {

                Debug.Log($"delivered item {deliveredItem.key} fulfilled.");
            }
            else
            {
                    Debug.Log($"cannot fulfill {deliveredItem.key}.");
            }
        }
        private void GetActiveOrders()
        {
            var orders = _orderService.GetActiveOrders();
            Debug.Log($"Active orders count: {orders.Count}.");
            foreach (var order in orders)
            {
                Debug.Log($"order id: {order.Id},\n " +
                    $"order items: {order.OrderItems.Count},\n " +
                    $"order max time: {order.MaxTime},\n " +
                    $"order status: {order.OrderStatus},\n " +
                    $"order remaining time: {order.RemainingTime}");
            }

        }
    }
}

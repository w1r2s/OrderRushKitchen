using OrderRushKitchen.Level;
using OrderRushKitchen.Sdk;
using System;
using System.Collections.Generic;
using Zenject;

namespace OrderRushKitchen.Order
{
    public sealed class OrderAnalyticsController : IInitializable, IDisposable
    {
        private readonly IOrderService _orderService;
        private readonly ILevelProgressionService _levelProgressionService;
        private readonly IAnalyticsService _analyticsService;

        public OrderAnalyticsController(IOrderService orderService, ILevelProgressionService levelProgressionService, IAnalyticsService analyticsService)
        {
            _orderService = orderService;
            _levelProgressionService = levelProgressionService;
            _analyticsService = analyticsService;
        }

        public void Initialize()
        {
            _orderService.OnOrderCompleted += OrderService_OnOrderCompleted;
            _orderService.OnOrderFailed += OrderService_OnOrderFailed;
        }

        public void Dispose()
        {
            _orderService.OnOrderCompleted -= OrderService_OnOrderCompleted;
            _orderService.OnOrderFailed -= OrderService_OnOrderFailed;
        }

        private void OrderService_OnOrderCompleted(object sender, OrderServiceEventArgs e)
        {
            _analyticsService.LogEvent(AnalyticsEvents.OrderCompleted, CreateOrderParameters(e.Order, string.Empty));
        }

        private void OrderService_OnOrderFailed(object sender, OrderServiceEventArgs e)
        {
            _analyticsService.LogEvent(AnalyticsEvents.OrderFailed, CreateOrderParameters(e.Order, "time_expired"));
        }

        private Dictionary<string, object> CreateOrderParameters(ActiveOrder order, string failureReason)
        {
            return new Dictionary<string, object>
            {
                [AnalyticsParameters.LevelIndex] = _levelProgressionService.CurrentLevelIndex,
                [AnalyticsParameters.ActiveOrdersCount] = _orderService.GetActiveOrders().Count,
                [AnalyticsParameters.OrderItemsCount] = order?.OrderItems.Count ?? 0,
                [AnalyticsParameters.CompletedItemsCount] = order?.CompletedItems ?? 0,
                [AnalyticsParameters.FailureReason] = failureReason
            };
        }
    }
}
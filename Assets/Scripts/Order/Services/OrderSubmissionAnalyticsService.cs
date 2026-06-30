using OrderRushKitchen.Level;
using OrderRushKitchen.Menu;
using OrderRushKitchen.Sdk;
using System.Collections.Generic;

namespace OrderRushKitchen.Order
{
    public sealed class OrderSubmissionAnalyticsService : IOrderSubmissionService
    {
        private readonly IOrderSubmissionService _inner;
        private readonly IOrderService _orderService;
        private readonly ILevelProgressionService _levelProgressionService;
        private readonly IAnalyticsService _analyticsService;

        public OrderSubmissionAnalyticsService(IOrderSubmissionService inner, IOrderService orderService, ILevelProgressionService levelProgressionService, IAnalyticsService analyticsService)
        {
            _inner = inner;
            _orderService = orderService;
            _levelProgressionService = levelProgressionService;
            _analyticsService = analyticsService;
        }

        public OrderSubmissionResult TrySubmit(MenuItemDefinitionSo menuItem)
        {
            OrderSubmissionResult result = _inner.TrySubmit(menuItem);
            LogSubmissionResult(result);
            return result;
        }

        public OrderSubmissionResult TrySubmit(MenuItemDefinitionSo menuItem, ActiveOrder targetOrder)
        {
            OrderSubmissionResult result = _inner.TrySubmit(menuItem, targetOrder);
            LogSubmissionResult(result);
            return result;
        }

        private void LogSubmissionResult(OrderSubmissionResult result)
        {
            if (result == null)
            {
                return;
            }

            string eventName = result.Success ? AnalyticsEvents.OrderSubmissionSucceeded : AnalyticsEvents.OrderSubmissionFailed;

            _analyticsService.LogEvent(eventName, CreateParameters(result));
        }

        private Dictionary<string, object> CreateParameters(OrderSubmissionResult result)
        {
            ActiveOrder order = result.Order;

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                [AnalyticsParameters.LevelIndex] = _levelProgressionService.CurrentLevelIndex,
                [AnalyticsParameters.ActiveOrdersCount] = _orderService.GetActiveOrders().Count,
                [AnalyticsParameters.OrderItemsCount] = order?.OrderItems.Count ?? 0,
                [AnalyticsParameters.CompletedItemsCount] = order?.CompletedItems ?? 0,
                [AnalyticsParameters.SubmissionResult] = result.Success ? "succeeded" : "failed"
            };

            if (!result.Success)
            {
                parameters[AnalyticsParameters.FailureReason] = result.FailureReason.ToString();
            }

            return parameters;
        }
    }
}
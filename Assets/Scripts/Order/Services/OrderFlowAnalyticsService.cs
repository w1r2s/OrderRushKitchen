using OrderRushKitchen.Level;
using OrderRushKitchen.Sdk;
using System.Collections.Generic;

namespace OrderRushKitchen.Order
{
    public sealed class OrderFlowAnalyticsService : IOrderFlowService
    {
        private readonly IOrderFlowService _inner;
        private readonly IOrderService _orderService;
        private readonly ILevelProgressionService _levelProgressionService;
        private readonly IAnalyticsService _analyticsService;

        public OrderFlowAnalyticsService(IOrderFlowService inner, IOrderService orderService, ILevelProgressionService levelProgressionService, IAnalyticsService analyticsService)
        {
            _inner = inner;
            _orderService = orderService;
            _levelProgressionService = levelProgressionService;
            _analyticsService = analyticsService;
        }

        public OrderFlowResult TryCreateOrder()
        {
            OrderFlowResult result = _inner.TryCreateOrder();

            if (result.Success)
            {
                _analyticsService.LogEvent(AnalyticsEvents.OrderAcceptSucceeded, CreateSuccessParameters(result.Order));
            }
            else
            {
                _analyticsService.LogEvent(AnalyticsEvents.OrderAcceptFailed, CreateFailureParameters(result.FailureReason));
            }

            return result;
        }

        private Dictionary<string, object> CreateSuccessParameters(ActiveOrder order)
        {
            Dictionary<string, object> parameters = CreateBaseParameters();

            parameters[AnalyticsParameters.OrderItemsCount] = order?.OrderItems.Count ?? 0;
            parameters[AnalyticsParameters.CompletedItemsCount] = order?.CompletedItems ?? 0;

            return parameters;
        }

        private Dictionary<string, object> CreateFailureParameters(OrderFlowFailureReason? failureReason)
        {
            Dictionary<string, object> parameters = CreateBaseParameters();

            parameters[AnalyticsParameters.FailureReason] = failureReason.HasValue ? failureReason.Value.ToString() : "unknown";

            return parameters;
        }

        private Dictionary<string, object> CreateBaseParameters()
        {
            return new Dictionary<string, object>
            {
                [AnalyticsParameters.LevelIndex] = _levelProgressionService.CurrentLevelIndex,
                [AnalyticsParameters.ActiveOrdersCount] = _orderService.GetActiveOrders().Count,
                [AnalyticsParameters.OrderItemsCount] = 0,
                [AnalyticsParameters.CompletedItemsCount] = 0
            };
        }
    }
}
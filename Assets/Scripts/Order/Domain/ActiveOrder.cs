using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Order
{
    public class ActiveOrder
    {
        private readonly string _id;
        private readonly List<OrderItem> _orderItems;
        private float _remainingTime;
        private float _maxTime;
        private OrderStatus _orderStatus;

        public string Id => _id;
        public IReadOnlyList<OrderItem> OrderItems =>_orderItems;
        public float RemainingTime => _remainingTime;
        public float MaxTime => _maxTime;
        public OrderStatus OrderStatus => _orderStatus;

        public bool IsActive => OrderStatus == OrderStatus.Active;
        public bool IsCompleted => OrderStatus == OrderStatus.Completed;
        public bool IsFailed => OrderStatus == OrderStatus.Failed;

        public float ProgressNormalized => _maxTime <= 0f ? 0f : _remainingTime / _maxTime;

        public int CompletedItems => _orderItems.Count(item => item.IsCompleted);
        public float InactiveElapsed { get; set; } = 0f;

        public ActiveOrder(string id, IEnumerable<MenuItemDefinitionSo> menuItems)
        {
            _id = id;
            _orderItems = new List<OrderItem>();

            foreach (var item in menuItems)
            {
                if (item == null)
                    continue;

                _orderItems.Add(new OrderItem(item));
                _maxTime += item.preparationTime;
            }
            _remainingTime = _maxTime;

            _orderStatus = OrderStatus.Active;
        }

        public void Tick(float deltaTime)
        {
            if (_orderStatus != OrderStatus.Active)
                return;

            _remainingTime -= deltaTime;
            if (_remainingTime <= 0)
            {
                _remainingTime = 0;
                _orderStatus = OrderStatus.Failed;
                return;
            }
        }
        public bool TryFulfill(MenuItemDefinitionSo deliveredItem)
        {
            if (_orderStatus != OrderStatus.Active)
                return false;

            if (deliveredItem == null)
                return false;

            var found = false;
            foreach (var item in _orderItems)
            {
                if (item.IsCompleted)
                    continue;
                if (!item.CanBeFulfilledBy(deliveredItem))
                    continue;
                item.MarkCompleted();
                found = true;
                break;
            }

            if (!found)
                return false;

            if (_orderItems.All(item => item.IsCompleted == true))
            {
                _orderStatus = OrderStatus.Completed;
            }
            return true;
        }
    }
}

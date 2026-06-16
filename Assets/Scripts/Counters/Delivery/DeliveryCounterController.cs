using OrderRushKitchen.Menu;
using OrderRushKitchen.Order;
using System.Collections.Generic;

namespace OrderRushKitchen.Counters
{
    public sealed class DeliveryCounterController
    {
        private readonly IOrderSubmissionService _submissionService;
        private readonly IOrderService _orderService;
        private readonly IOrderStagingReservationService _reservationService;
        private readonly List<OrderItem> _stagedOrderItems = new();

        private ActiveOrder _boundOrder;
        private OrderStagingReservation _reservation;
        private ActiveOrder _pendingResolvedOrder;
        private DeliveryCounterState _state;
        private bool _submissionInProgress;

        public ActiveOrder BoundOrder => _boundOrder;
        public DeliveryCounterState State => _state;
        public bool IsSubmissionInProgress => _submissionInProgress;

        public DeliveryCounterController(IOrderSubmissionService submissionService, IOrderService orderService, IOrderStagingReservationService reservationService)
        {
            _submissionService = submissionService;
            _orderService = orderService;
            _reservationService = reservationService;
        }

        public bool TryPrepareSubmission(MenuItemDefinitionSo menuItem, out OrderStagingReservation reservation, out bool isNewReservation)
        {
            reservation = _reservation;
            isNewReservation = false;

            if (_state == DeliveryCounterState.Staging)
                return reservation?.Order != null;

            if (_state != DeliveryCounterState.Unbound)
                return false;

            if (!_reservationService.TryReserveFor(menuItem, out reservation))
                return false;

            isNewReservation = true;
            return true;
        }

        public void ReleasePreparedReservation(OrderStagingReservation reservation, bool isNewReservation)
        {
            if (isNewReservation)
            {
                _reservationService.Release(reservation);
            }
        }

        public OrderSubmissionResult SubmitPrepared(MenuItemDefinitionSo menuItem, OrderStagingReservation reservation)
        {
            _submissionInProgress = true;

            try
            {
                return _submissionService.TrySubmit(menuItem, reservation.Order);
            }
            finally
            {
                _submissionInProgress = false;
            }
        }

        public bool RegisterSuccessfulSubmission(OrderSubmissionResult result, OrderStagingReservation reservation, out bool submittedToUnexpectedOrder)
        {
            submittedToUnexpectedOrder = false;

            if (result == null || !result.Success)
                return false;

            if (_state == DeliveryCounterState.Unbound)
            {
                _reservation = reservation;
                _boundOrder = result.Order;
                _state = DeliveryCounterState.Staging;
            }

            submittedToUnexpectedOrder = !ReferenceEquals(_boundOrder, result.Order);
            _stagedOrderItems.Add(result.FulfilledItem);
            return true;
        }

        public bool TryHandleFailedSubmission(OrderSubmissionResult result)
        {
            if (result?.FailureReason != OrderSubmissionFailureReason.TargetOrderUnavailable)
                return false;

            return TryBeginResolvingCurrentOrder();
        }

        public bool TryBeginResolvingCurrentOrder()
        {
            return TryBeginResolving(_boundOrder);
        }

        public bool HandleOrderResolved(ActiveOrder order)
        {
            if (order == null)
                return false;

            if (_submissionInProgress)
            {
                _pendingResolvedOrder = order;
                return false;
            }

            return TryBeginResolving(order);
        }

        public bool TryProcessPendingResolution()
        {
            if (_pendingResolvedOrder == null)
                return false;

            ActiveOrder resolvedOrder = _pendingResolvedOrder;
            _pendingResolvedOrder = null;

            return TryBeginResolving(resolvedOrder);
        }

        public bool HandleOrderRemoved(ActiveOrder removedOrder)
        {
            if (removedOrder == null)
                return false;

            if (ReferenceEquals(_pendingResolvedOrder, removedOrder))
            {
                _pendingResolvedOrder = null;
            }

            if (!ReferenceEquals(_boundOrder, removedOrder))
                return false;

            if (_state == DeliveryCounterState.Resolving)
                return false;

            Reset();
            return true;
        }

        public bool TryRollbackStagedDeliveries()
        {
            if (_boundOrder == null || _stagedOrderItems.Count == 0)
                return false;

            var orderItems = new List<OrderItem>();

            for (int i = 0; i < _stagedOrderItems.Count; i++)
            {
                OrderItem orderItem = _stagedOrderItems[i];

                if (orderItem == null)
                    return false;

                orderItems.Add(orderItem);
            }

            return _orderService.TryRevokeFulfilledItems(_boundOrder, orderItems);
        }

        public void CompleteResolving()
        {
            Reset();
        }

        public void Reset()
        {
            ReleaseReservation();
            _boundOrder = null;
            _pendingResolvedOrder = null;
            _submissionInProgress = false;
            _state = DeliveryCounterState.Unbound;
            _stagedOrderItems.Clear();
        }

        private bool TryBeginResolving(ActiveOrder order)
        {
            if (order == null || !ReferenceEquals(order, _boundOrder) || _state == DeliveryCounterState.Resolving)
                return false;

            _state = DeliveryCounterState.Resolving;
            return true;
        }

        private void ReleaseReservation()
        {
            if (_reservation == null)
                return;

            _reservationService.Release(_reservation);
            _reservation = null;
        }
    }
}

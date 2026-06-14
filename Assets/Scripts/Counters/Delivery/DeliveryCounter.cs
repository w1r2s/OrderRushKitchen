using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Menu;
using OrderRushKitchen.Order;
using OrderRushKitchen.PlayerControl;
using OrderRushKitchen.Serving;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Counters
{
    public class DeliveryCounter : BaseCounter
    {
        public event EventHandler OnDeliverySuccess;
        public event EventHandler OnDeliveryFail;

        [SerializeField] private List<DeliveryStagingSlot> stagingSlots = new();

        private readonly List<StagedDelivery> _stagedDeliveries = new();

        private IOrderSubmissionService _submissionService;
        private IOrderService _orderService;
        private IOrderStagingReservationService _reservationService;

        private ActiveOrder _boundOrder;
        private OrderStagingReservation _reservation;
        private ActiveOrder _pendingResolvedOrder;
        private DeliveryCounterState _state;
        private bool _submissionInProgress;

        public ActiveOrder BoundOrder => _boundOrder;
        public DeliveryCounterState State => _state;

        [Inject]
        public void Construct(IOrderSubmissionService submissionService, IOrderService orderService, IOrderStagingReservationService reservationService)
        {
            _submissionService = submissionService;
            _orderService = orderService;
            _reservationService = reservationService;
        }

        private void Start()
        {
            _orderService.OnOrderCompleted += OrderService_OnOrderCompleted;
            _orderService.OnOrderFailed += OrderService_OnOrderFailed;
            _orderService.OnOrderRemoved += OrderService_OnOrderRemoved;
        }

        private void OnDestroy()
        {
            if (_orderService != null)
            {
                _orderService.OnOrderCompleted -= OrderService_OnOrderCompleted;
                _orderService.OnOrderFailed -= OrderService_OnOrderFailed;
                _orderService.OnOrderRemoved -= OrderService_OnOrderRemoved;
            }

            ReleaseReservation();
        }

        public override void Interact(Player player)
        {
            if (player == null || _submissionInProgress || _state == DeliveryCounterState.Resolving || !player.HasObject)
            {
                return;
            }

            if (!TryGetMenuItem(player, out var menuItem))
            {
                NotifyDeliveryFailed();
                return;
            }

            if (!TryGetFreeSlot(out var slot))
            {
                NotifyDeliveryFailed();
                return;
            }

            if (!TryGetSubmissionReservation(menuItem, out OrderStagingReservation reservation, out bool isNewReservation))
            {
                NotifyDeliveryFailed();
                return;
            }

            if (!player.TryTransferObjectTo(slot))
            {
                ReleaseNewReservation(reservation, isNewReservation);
                NotifyDeliveryFailed();
                return;
            }

            _submissionInProgress = true;

            OrderSubmissionResult result;

            try
            {
                result = _submissionService.TrySubmit(menuItem, reservation.Order);
            }
            finally
            {
                _submissionInProgress = false;
            }

            if (!result.Success)
            {
                RollbackTransfer(slot, player);
                ReleaseNewReservation(reservation, isNewReservation);

                if (result.FailureReason == OrderSubmissionFailureReason.TargetOrderUnavailable)
                {
                    BeginResolving(_boundOrder);
                }

                NotifyDeliveryFailed();
                return;
            }

            RegisterSuccessfulDelivery(slot, result, reservation);
            ProcessPendingResolution();

            OnDeliverySuccess?.Invoke(this, EventArgs.Empty);
        }

        public override bool TryInteractAlternate(Player player)
        {
            return false;
        }

        public override void ResetForLevelTransition()
        {
            base.ResetForLevelTransition();

            CleanupStagingSlots();
            ResetStagingState();
        }

        private bool TryGetMenuItem(Player player, out MenuItemDefinitionSo menuItem)
        {
            menuItem = null;

            KitchenObject heldObject = player.GetObject();

            if (heldObject is not ISubmittableMenuItemSource source)
                return false;

            return source.TryGetMenuItemForSubmit(out menuItem) && menuItem != null;
        }

        private bool TryGetFreeSlot(out DeliveryStagingSlot freeSlot)
        {
            freeSlot = null;

            for (int i = 0; i < stagingSlots.Count; i++)
            {
                DeliveryStagingSlot slot = stagingSlots[i];

                if (slot == null || slot.HasObject)
                    continue;

                freeSlot = slot;
                return true;
            }

            return false;
        }

        private bool TryGetSubmissionReservation(MenuItemDefinitionSo menuItem, out OrderStagingReservation reservation, out bool isNewReservation)
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

        private void RegisterSuccessfulDelivery(DeliveryStagingSlot slot, OrderSubmissionResult result, OrderStagingReservation reservation)
        {
            if (_state == DeliveryCounterState.Unbound)
            {
                _reservation = reservation;
                _boundOrder = result.Order;
                _state = DeliveryCounterState.Staging;
            }

            if (!ReferenceEquals(_boundOrder, result.Order))
            {
                Debug.LogError($"{name}: submitted item was fulfilled by an unexpected order.");
            }

            _stagedDeliveries.Add(new StagedDelivery(slot, result.FulfilledItem));
        }

        private void ReleaseNewReservation(OrderStagingReservation reservation, bool isNewReservation)
        {
            if (isNewReservation)
            {
                _reservationService.Release(reservation);
            }
        }

        private void RollbackTransfer(DeliveryStagingSlot slot, Player player)
        {
            if (slot.TryTransferObjectTo(player))
                return;

            Debug.LogError($"{name}: failed to return rejected delivery to player.");
        }

        private void OrderService_OnOrderCompleted(object sender, OrderServiceEventArgs e)
        {
            HandleOrderResolved(e?.Order);
        }

        private void OrderService_OnOrderFailed(object sender, OrderServiceEventArgs e)
        {
            HandleOrderResolved(e?.Order);
        }

        private void OrderService_OnOrderRemoved(object sender, OrderServiceEventArgs e)
        {
            ActiveOrder removedOrder = e?.Order;

            if (removedOrder == null)
                return;

            if (ReferenceEquals(_pendingResolvedOrder, removedOrder))
            {
                _pendingResolvedOrder = null;
            }

            if (!ReferenceEquals(_boundOrder, removedOrder))
                return;

            CleanupStagingSlots();
            ResetStagingState();
        }

        private void HandleOrderResolved(ActiveOrder order)
        {
            if (order == null)
                return;

            if (_submissionInProgress)
            {
                _pendingResolvedOrder = order;
                return;
            }

            BeginResolving(order);
        }

        private void ProcessPendingResolution()
        {
            if (_pendingResolvedOrder == null)
                return;

            ActiveOrder resolvedOrder = _pendingResolvedOrder;
            _pendingResolvedOrder = null;

            BeginResolving(resolvedOrder);
        }

        private void BeginResolving(ActiveOrder order)
        {
            if (order == null || !ReferenceEquals(order, _boundOrder))
                return;

            _state = DeliveryCounterState.Resolving;
        }

        private void CleanupStagingSlots()
        {
            for (int i = 0; i < stagingSlots.Count; i++)
            {
                DeliveryStagingSlot slot = stagingSlots[i];

                if (slot == null)
                    continue;

                slot.TryRemoveAndDestroyObject();
            }

            _stagedDeliveries.Clear();
        }

        private void ResetStagingState()
        {
            ReleaseReservation();
            _boundOrder = null;
            _pendingResolvedOrder = null;
            _submissionInProgress = false;
            _state = DeliveryCounterState.Unbound;
        }

        private void ReleaseReservation()
        {
            if (_reservation == null)
                return;

            _reservationService?.Release(_reservation);
            _reservation = null;
        }

        private void NotifyDeliveryFailed()
        {
            OnDeliveryFail?.Invoke(this, EventArgs.Empty);
        }

        private sealed class StagedDelivery
        {
            public DeliveryStagingSlot Slot { get; }
            public OrderItem OrderItem { get; }

            public StagedDelivery(DeliveryStagingSlot slot, OrderItem orderItem)
            {
                Slot = slot;
                OrderItem = orderItem;
            }
        }
    }
}

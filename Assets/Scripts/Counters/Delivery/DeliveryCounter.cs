using OrderRushKitchen.Game;
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

        private readonly List<DeliveryStagingSlot> _stagedSlots = new();

        private IOrderService _orderService;
        private IGameClock _gameClock;
        private DeliveryCounterController _controller;

        public ActiveOrder BoundOrder => _controller?.BoundOrder;
        public DeliveryCounterState State => _controller?.State ?? DeliveryCounterState.Unbound;

        [Inject]
        public void Construct(IOrderSubmissionService submissionService, IOrderService orderService, IOrderStagingReservationService reservationService, IGameClock gameClock)
        {
            _orderService = orderService;
            _gameClock = gameClock;
            _controller = new DeliveryCounterController(submissionService, orderService, reservationService);
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

            _controller?.Reset();
        }

        private void Update()
        {
            if (State != DeliveryCounterState.Resolving)
                return;

            TickResolveStaging();
        }

        public override void Interact(Player player)
        {
            if (player == null || _controller.IsSubmissionInProgress || State == DeliveryCounterState.Resolving || !player.HasObject)
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

            if (!_controller.TryPrepareSubmission(menuItem, out OrderStagingReservation reservation, out bool isNewReservation))
            {
                NotifyDeliveryFailed();
                return;
            }

            if (!player.TryTransferObjectTo(slot))
            {
                _controller.ReleasePreparedReservation(reservation, isNewReservation);
                NotifyDeliveryFailed();
                return;
            }

            OrderSubmissionResult result = _controller.SubmitPrepared(menuItem, reservation);

            if (!result.Success)
            {
                RollbackTransfer(slot, player);
                _controller.ReleasePreparedReservation(reservation, isNewReservation);

                if (_controller.TryHandleFailedSubmission(result))
                {
                    BeginResolvePresentation();
                }

                NotifyDeliveryFailed();
                return;
            }

            RegisterSuccessfulDelivery(slot, result, reservation);

            OnDeliverySuccess?.Invoke(this, EventArgs.Empty);
        }

        public override bool TryInteractAlternate(Player player)
        {
            if (State != DeliveryCounterState.Staging || BoundOrder == null || _stagedSlots.Count == 0)
                return false;

            if (!_controller.TryRollbackStagedDeliveries())
                return false;

            if (_controller.TryBeginResolvingCurrentOrder())
            {
                BeginResolvePresentation();
            }

            return true;
        }

        public override void ResetForLevelTransition()
        {
            base.ResetForLevelTransition();

            CleanupStagingSlots();
            _controller.Reset();
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

        private void RegisterSuccessfulDelivery(DeliveryStagingSlot slot, OrderSubmissionResult result, OrderStagingReservation reservation)
        {
            if (!_controller.RegisterSuccessfulSubmission(result, reservation, out bool submittedToUnexpectedOrder))
                return;

            if (submittedToUnexpectedOrder)
            {
                Debug.LogError($"{name}: submitted item was fulfilled by an unexpected order.");
            }

            if (!slot.TryApplyStagedPresentation())
            {
                Debug.LogWarning($"{name}: staged object presentation could not be applied.");
            }

            _stagedSlots.Add(slot);

            if (_controller.TryProcessPendingResolution())
            {
                BeginResolvePresentation();
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
            if (_controller.HandleOrderResolved(e?.Order))
            {
                BeginResolvePresentation();
            }
        }

        private void OrderService_OnOrderFailed(object sender, OrderServiceEventArgs e)
        {
            if (_controller.HandleOrderResolved(e?.Order))
            {
                BeginResolvePresentation();
            }
        }

        private void OrderService_OnOrderRemoved(object sender, OrderServiceEventArgs e)
        {
            if (_controller.HandleOrderRemoved(e?.Order))
            {
                CleanupStagingSlots();
            }
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

            _stagedSlots.Clear();
        }

        private void NotifyDeliveryFailed()
        {
            OnDeliveryFail?.Invoke(this, EventArgs.Empty);
        }

        private void BeginResolvePresentation()
        {
            for (int i = 0; i < _stagedSlots.Count; i++)
            {
                DeliveryStagingSlot slot = _stagedSlots[i];

                if (slot == null)
                    continue;

                slot.TryBeginResolvePresentation();
            }
        }

        private void TickResolveStaging()
        {
            bool allResolved = true;
            float deltaTime = _gameClock?.DeltaTime ?? Time.deltaTime;

            for (int i = 0; i < _stagedSlots.Count; i++)
            {
                DeliveryStagingSlot slot = _stagedSlots[i];

                if (slot == null)
                    continue;

                if (!slot.TickResolvePresentation(deltaTime))
                {
                    allResolved = false;
                }
            }

            if (!allResolved)
                return;

            CleanupStagingSlots();
            _controller.CompleteResolving();
        }
    }
}

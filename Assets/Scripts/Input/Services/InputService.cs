using OrderRushKitchen.PlayerControl;
using System;
using UnityEngine.InputSystem;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Input
{
    public class InputService : IGameplayInputService, IInitializable, IDisposable
    {
        private readonly Actions _actions;

        public event EventHandler OnInteractAction;
        public event EventHandler OnInteractAlternateAction;
        public event EventHandler OnPauseAction;

        [Inject]
        public InputService(Actions actions)
        {
            _actions = actions;
        }
        public void Initialize()
        {
            _actions.Player.Enable();

            _actions.Player.Interact.performed += Interact_performed;
            _actions.Player.InteractAlternate.performed += InteractAlternate_performed;
            _actions.Player.Pause.performed += Pause_performed;
        }
        public void Dispose()
        {
            _actions.Player.Interact.performed -= Interact_performed;
            _actions.Player.InteractAlternate.performed -= InteractAlternate_performed;
            _actions.Player.Pause.performed -= Pause_performed;

            _actions.Player.Disable();
        }
        private void Pause_performed(InputAction.CallbackContext obj)
        {
            OnPauseAction?.Invoke(this, EventArgs.Empty);
        }

        private void InteractAlternate_performed(InputAction.CallbackContext obj)
        {
            OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
        }

        private void Interact_performed(InputAction.CallbackContext obj)
        {
            OnInteractAction?.Invoke(this, EventArgs.Empty);
        }
        public Vector2 GetMovementVector()
        {
            Vector2 inputVector = _actions.Player.Movement.ReadValue<Vector2>();

            return Vector2.ClampMagnitude(inputVector, 1f);
        }
    }
}

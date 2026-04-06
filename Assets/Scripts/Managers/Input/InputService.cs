using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Managers.Input
{
    public class InputService : IInputService, IDisposable
    {
        private readonly Actions _actions;
        private readonly IInputStorage _storage;

        public event EventHandler OnInteractAction;
        public event EventHandler OnInteractAlternateAction;
        public event EventHandler OnPauseAction;
        public InputService(Actions actions, IInputStorage storage)
        {
            _actions = actions;
            _storage = storage;

            var json = storage.LoadBindings();
            if (!string.IsNullOrEmpty(json))
            {
                _actions.LoadBindingOverridesFromJson(json);
            }

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

            _actions.Disable();

            _actions.Dispose();
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
        public Vector2 GetMovementVectorNormalized()
        {
            Vector2 inputVector = _actions.Player.Movement.ReadValue<Vector2>();

            inputVector = inputVector.normalized;

            return inputVector;
        }
        public string GetKeyBindingText(InputKeyBinding binding)
        {
            switch (binding)
            {
                default:
                case InputKeyBinding.Move_Up:
                    return _actions.Player.Movement.bindings[1].ToDisplayString();
                case InputKeyBinding.Move_Down:
                    return _actions.Player.Movement.bindings[2].ToDisplayString();
                case InputKeyBinding.Move_Left:
                    return _actions.Player.Movement.bindings[3].ToDisplayString();
                case InputKeyBinding.Move_Right:
                    return _actions.Player.Movement.bindings[4].ToDisplayString();
                case InputKeyBinding.Interact:
                    return _actions.Player.Interact.bindings[0].ToDisplayString();
                case InputKeyBinding.Alt_Interact:
                    return _actions.Player.InteractAlternate.bindings[0].ToDisplayString();
                case InputKeyBinding.Pause:
                    return _actions.Player.Pause.bindings[0].ToDisplayString();
            }
        }
        public void RebindKeyBinding(InputKeyBinding binding, Action onActionRebound)
        {
            InputAction inputAction;
            int actIndex;

            switch (binding)
            {
                default:
                case InputKeyBinding.Move_Up:
                    inputAction = _actions.Player.Movement;
                    actIndex = 1;
                    break;
                case InputKeyBinding.Move_Down:
                    inputAction = _actions.Player.Movement;
                    actIndex = 2;
                    break;
                case InputKeyBinding.Move_Left:
                    inputAction = _actions.Player.Movement;
                    actIndex = 3;
                    break;
                case InputKeyBinding.Move_Right:
                    inputAction = _actions.Player.Movement;
                    actIndex = 4;
                    break;
                case InputKeyBinding.Interact:
                    inputAction = _actions.Player.Interact;
                    actIndex = 0;
                    break;
                case InputKeyBinding.Alt_Interact:
                    inputAction = _actions.Player.InteractAlternate;
                    actIndex = 0;
                    break;
                case InputKeyBinding.Pause:
                    inputAction = _actions.Player.Pause;
                    actIndex = 0;
                    break;

            }
            _actions.Player.Disable();

            inputAction.PerformInteractiveRebinding(actIndex)
                .OnComplete(callback =>
                {
                    callback.Dispose();
                    _actions.Player.Enable();
                    onActionRebound();

                    _storage.SaveBindings(_actions.SaveBindingOverridesAsJson());
                })
                .Start();
        }
    }
}

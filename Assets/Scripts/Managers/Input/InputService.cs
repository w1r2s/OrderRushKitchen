using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Scripts.Managers.Input
{
    public class InputService : IGameplayInputService, IInputRebindingService, IInitializable, IDisposable
    {
        private readonly struct BindingTarget
        {
            public readonly InputAction Action;
            public readonly int BindingIndex;

            public BindingTarget(InputAction action, int bindingIndex)
            {
                Action = action;
                BindingIndex = bindingIndex;
            }
        }
        private InputActionRebindingExtensions.RebindingOperation _activeRebindOperation;
        private readonly Actions _actions;
        private readonly IInputStorage _storage;

        public event EventHandler OnInteractAction;
        public event EventHandler OnInteractAlternateAction;
        public event EventHandler OnPauseAction;

        [Inject]
        public InputService(Actions actions, IInputStorage storage)
        {
            _actions = actions;
            _storage = storage;
        }
        public void Initialize()
        {
            var json = _storage.LoadBindings();
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
            _activeRebindOperation?.Dispose();
            _activeRebindOperation = null;

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
        public Vector2 GetMovementVector()
        {
            Vector2 inputVector = _actions.Player.Movement.ReadValue<Vector2>();

            return Vector2.ClampMagnitude(inputVector, 1f);
        }
        public string GetKeyBindingText(InputKeyBinding binding)
        {
            var target = GetBindingTarget(binding);
            return FormatBindingDisplayText(target.Action.bindings[target.BindingIndex].ToDisplayString());
        }

        public void RebindKeyBinding(InputKeyBinding binding, Action<bool> onFinished)
        {
            if (_activeRebindOperation != null)
                return;

            var target = GetBindingTarget(binding);

            _actions.Player.Disable();

            var operation = target.Action.PerformInteractiveRebinding(target.BindingIndex);

            if (binding != InputKeyBinding.Pause)
            {
                operation.WithCancelingThrough("<Keyboard>/escape");
            }

            _activeRebindOperation = operation;

            operation
                .OnComplete(callback =>
                {
                    callback.Dispose();
                    _activeRebindOperation = null;

                    _actions.Player.Enable();

                    _storage.SaveBindings(_actions.SaveBindingOverridesAsJson());
                    onFinished?.Invoke(true);
                })
                .OnCancel(callback =>
                {
                    callback.Dispose();
                    _activeRebindOperation = null;

                    _actions.Player.Enable();

                    onFinished?.Invoke(false);
                })
                .Start();
        }

        private BindingTarget GetBindingTarget(InputKeyBinding binding)
        {
            return binding switch
            {
                InputKeyBinding.Move_Up => FindCompositePartBinding(_actions.Player.Movement, "up"),
                InputKeyBinding.Move_Down => FindCompositePartBinding(_actions.Player.Movement, "down"),
                InputKeyBinding.Move_Left => FindCompositePartBinding(_actions.Player.Movement, "left"),
                InputKeyBinding.Move_Right => FindCompositePartBinding(_actions.Player.Movement, "right"),
                InputKeyBinding.Interact => FindKeyboardBinding(_actions.Player.Interact),
                InputKeyBinding.Alt_Interact => FindKeyboardBinding(_actions.Player.InteractAlternate),
                InputKeyBinding.Pause => FindKeyboardBinding(_actions.Player.Pause),
                _ => throw new ArgumentOutOfRangeException(nameof(binding), binding, null)
            };
        }

        private BindingTarget FindCompositePartBinding(InputAction action, string partName)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                var binding = action.bindings[i];

                if (!binding.isPartOfComposite)
                    continue;

                if (!string.Equals(binding.name, partName, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!BindingGroupsContain(binding.groups, "KeyBoard"))
                    continue;

                return new BindingTarget(action, i);
            }

            throw new InvalidOperationException($"Keyboard composite binding part '{partName}' was not found for action '{action.name}'.");
        }

        private BindingTarget FindKeyboardBinding(InputAction action)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                var binding = action.bindings[i];

                if (binding.isComposite || binding.isPartOfComposite)
                    continue;

                if (!BindingGroupsContain(binding.groups, "KeyBoard"))
                    continue;

                return new BindingTarget(action, i);
            }

            throw new InvalidOperationException($"Keyboard binding was not found for action '{action.name}'.");
        }

        private static bool BindingGroupsContain(string groups, string targetGroup)
        {
            if (string.IsNullOrEmpty(groups))
                return false;

            var splitGroups = groups.Split(';');
            for (int i = 0; i < splitGroups.Length; i++)
            {
                if (string.Equals(splitGroups[i], targetGroup, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
        private static string FormatBindingDisplayText(string text)
        {
            return text switch
            {
                "Escape" => "Esc",
                _ => text
            };
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public enum KeyBinding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        Alt_Interact,
        Pause
    }
    private Actions actions;

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    private void Awake()
    {
        Instance = this;
        actions = new Actions();

        if (PlayerPrefs.HasKey("PlayerKeyBindings"))
        {
            actions.LoadBindingOverridesFromJson(PlayerPrefs.GetString("PlayerKeyBindings"));
        }
        actions.Player.Enable();

        actions.Player.Interact.performed += Interact_performed;
        actions.Player.InteractAlternate.performed += InteractAlternate_performed;
        actions.Player.Pause.performed += Pause_performed;
    }
    private void OnDestroy()
    {
        actions.Player.Interact.performed -= Interact_performed;
        actions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        actions.Player.Pause.performed -= Pause_performed;

        actions.Dispose();
    }
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = actions.Player.Movement.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetKeyBindingText(KeyBinding binding)
    {
        switch (binding)
        {
            default:
            case KeyBinding.Move_Up:
                return actions.Player.Movement.bindings[1].ToDisplayString();
            case KeyBinding.Move_Down:
                return actions.Player.Movement.bindings[2].ToDisplayString();
            case KeyBinding.Move_Left:
                return actions.Player.Movement.bindings[3].ToDisplayString();
            case KeyBinding.Move_Right:
                return actions.Player.Movement.bindings[4].ToDisplayString();
            case KeyBinding.Interact:
                return actions.Player.Interact.bindings[0].ToDisplayString();
            case KeyBinding.Alt_Interact:
                return actions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case KeyBinding.Pause:
                return actions.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindKeyBinding(KeyBinding binding, Action onActionRebound)
    {
        InputAction inputAction;
        int actIndex;

        switch (binding)
        {
            default:
            case KeyBinding.Move_Up:
                inputAction = actions.Player.Movement;
                actIndex = 1;
                break;
            case KeyBinding.Move_Down:
                inputAction = actions.Player.Movement;
                actIndex = 2;
                break;
            case KeyBinding.Move_Left:
                inputAction = actions.Player.Movement;
                actIndex = 3;
                break;
            case KeyBinding.Move_Right:
                inputAction = actions.Player.Movement;
                actIndex = 4;
                break;
            case KeyBinding.Interact:
                inputAction = actions.Player.Interact;
                actIndex = 0;
                break;
            case KeyBinding.Alt_Interact:
                inputAction = actions.Player.InteractAlternate;
                actIndex = 0;
                break;
            case KeyBinding.Pause:
                inputAction = actions.Player.Pause;
                actIndex = 0;
                break;

        }
        actions.Player.Disable();

        inputAction.PerformInteractiveRebinding(actIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                actions.Player.Enable();
                onActionRebound();

                PlayerPrefs.SetString("PlayerKeyBindings", actions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            })
            .Start();
    }
}

using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Actions actions;

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    private void Awake()
    {
        actions = new Actions();
        actions.Player.Enable();

        actions.Player.Interact.performed += Interact_performed;
        actions.Player.InteractAlternate.performed += InteractAlternate_performed;
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
}

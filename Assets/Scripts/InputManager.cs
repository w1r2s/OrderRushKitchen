using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Actions actions;

    public event EventHandler OnInteractAction;
    private void Awake()
    {
        actions = new Actions();
        actions.Player.Enable();

        actions.Player.Interact.performed += Interact_performed;
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

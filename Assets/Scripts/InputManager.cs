using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Actions actions;
    private void Awake()
    {
        actions = new Actions();
        actions.Player.Movement.Enable();
    }
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = actions.Player.Movement.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}

using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private InputManager inputManager;

    private bool isWalking;

    private void Awake()
    {
        inputManager = GameObject.FindFirstObjectByType<InputManager>();
    }
    void Update()
    {
        var inputVector = inputManager.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDist = moveSpeed * Time.deltaTime;
        float playerRarius = .7f;
        float playerHeight = .7f;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRarius, moveDir, moveDist);

        if (!canMove)
        {
           Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRarius, moveDirX, moveDist);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirY = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRarius, moveDirY, moveDist);
                if (canMove)
                {
                    moveDir = moveDirY;
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;

        }

        isWalking = moveDir != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}

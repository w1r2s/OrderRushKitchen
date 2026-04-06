using Assets.Scripts.Managers.Game;
using Assets.Scripts.Managers.Input;
using System;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour, IKitchenObjectParent
{

    public event EventHandler onPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform KitchenObjectHoldPoint;

    private IGameService _gameService;
    private IInputService _inputService;

    private Vector3 lastInteraction;
    private bool isWalking;

    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    [Inject]
    private void Construct(IGameService gameService, IInputService inputManager)
    {
        _gameService = gameService;
        _inputService = inputManager;

    }

    private void OnEnable()
    {
        _inputService.OnInteractAction += OnInteract;
        _inputService.OnInteractAlternateAction += OnInteractAlternate;
    }

    private void OnDisable()
    {
        if (_inputService == null) return;

        _inputService.OnInteractAction -= OnInteract;
        _inputService.OnInteractAlternateAction -= OnInteractAlternate;
    }

    private void Update()
    {
        if (!_gameService.IsGamePlaying())
            return;

        Vector2 input = _inputService.GetMovementVectorNormalized();

        HandleMovement(input);
        HandleInteractions(input);
    }

    private void OnInteractAlternate(object sender, EventArgs e)
    {
        if (!_gameService.IsGamePlaying())
            return;

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void OnInteract(object sender, System.EventArgs e)
    {
        if (!_gameService.IsGamePlaying())
            return;

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }
    private void HandleInteractions(Vector2 inputVector)
    {


        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteraction = moveDir;
        }


        float interactionDist = 2f;

        if (Physics.Raycast(transform.position, lastInteraction, out RaycastHit hitInfo, interactionDist, counterLayerMask))
        {
            if (hitInfo.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }

    }
    private void HandleMovement(Vector2 inputVector)
    {

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDist = moveSpeed * Time.deltaTime;
        float playerRarius = .7f;
        float playerHeight = .7f;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRarius, moveDir, moveDist);

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRarius, moveDirX, moveDist);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirY = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRarius, moveDirY, moveDist);
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

        if (isWalking)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return KitchenObjectHoldPoint;
    }

    public void SetKitchenObjcet(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            onPickedSomething?.Invoke(this, EventArgs.Empty);
        }

    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}

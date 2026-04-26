using Assets.Scripts.Managers.Game;
using Assets.Scripts.Managers.Input;
using Assets.Scripts.Managers.Sound;
using Assets.Scripts.Selection;
using System;
using UnityEngine;
using Zenject;

public class Player : ObjectHolder
{
    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private LayerMask counterLayerMask;

    private IGameService _gameService;
    private IInputService _inputService;
    private IAudioService _audioService;
    IItemSelectionService _selectionService;

    private Vector3 lastInteraction;
    private bool isWalking;

    private BaseCounter selectedCounter;

    [Inject]
    private void Construct(IGameService gameService, IInputService inputService, IAudioService audioService, IItemSelectionService selectionService)
    {
        _gameService = gameService;
        _inputService = inputService;
        _audioService = audioService;
        _selectionService = selectionService;

        _inputService.OnInteractAction += OnInteract;
        _inputService.OnInteractAlternateAction += OnInteractAlternate;
    }

    private void OnDestroy()
    {
        if (_inputService != null)
        {
            _inputService.OnInteractAction -= OnInteract;
            _inputService.OnInteractAlternateAction -= OnInteractAlternate;
        }
    }

    private void Update()
    {
        if (!_gameService.IsGamePlaying())
            return;

        if (_selectionService.IsOpen)
            return;

        Vector2 input = _inputService.GetMovementVectorNormalized();

        HandleMovement(input);
        HandleInteractions(input);
    }

    private void OnInteract(object sender, EventArgs e)
    {
        if (!_gameService.IsGamePlaying())
            return;

        if (_selectionService.IsOpen)
            return;
        selectedCounter?.Interact(this);
    }

    private void OnInteractAlternate(object sender, EventArgs e)
    {
        if (!_gameService.IsGamePlaying())
            return;

        if (_selectionService.IsOpen)
            return;

        selectedCounter?.InteractAlternate(this);
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
        float playerRadius = .7f;
        float playerHeight = .7f;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDist);

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;

            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDist);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirY = new Vector3(0, 0, moveDir.z).normalized;

                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirY, moveDist);

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
        if (this.selectedCounter == selectedCounter)
            return;

        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public override void SetObject(KitchenObject obj)
    {
        base.SetObject(obj);

        if (obj != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
            _audioService.PlayPickUp(transform.position);
        }
    }

    public bool IsWalking() => isWalking;
}
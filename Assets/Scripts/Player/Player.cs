using Assets.Scripts.Managers.Game;
using Assets.Scripts.Managers.Input;
using Assets.Scripts.Managers.Sound;
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

    [Header("Movement")]
    [SerializeField, Min(0.01f)] private float playerRadius = 0.35f;
    [SerializeField, Min(0.01f)] private float playerHeight = 2f;
    [SerializeField, Min(0f)] private float moveSpeed = 5f;
    [SerializeField, Min(0f)] private float rotationSpeed = 10f;
    [SerializeField] private LayerMask movementCollisionMask;

    [Header("Interaction")]
    [SerializeField, Min(0f)] private float interactionDistance = 2f;
    [SerializeField, Min(0f)] private float interactionRayHeight = 0.8f;
    [SerializeField] private LayerMask counterLayerMask;


    private IGameService _gameService;
    private IGameplayInputService _inputService;
    private IAudioService _audioService;
    private IGamePauseService _pauseService;
    private IGameClock _clock;

    private Vector3 lastInteraction;
    private bool isWalking;

    private BaseCounter selectedCounter;

    [Inject]
    private void Construct(IGameService gameService, IGameplayInputService inputService, IAudioService audioService, IGamePauseService pauseService, IGameClock clock)
    {
        _gameService = gameService;
        _inputService = inputService;
        _audioService = audioService;
        _pauseService = pauseService;
        _clock = clock;

        _inputService.OnInteractAction += OnInteract;
        _inputService.OnInteractAlternateAction += OnInteractAlternate;

        lastInteraction = transform.forward;
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
        if (!CanHandleGameplayInput())
        {
            isWalking = false;
            SetSelectedCounter(null);
            return;
        }

        Vector2 input = _inputService.GetMovementVector();

        HandleMovement(input);
        HandleInteractions(input);
    }

    private void OnInteract(object sender, EventArgs e)
    {
        if (!CanHandleGameplayInput())
            return;

        selectedCounter?.Interact(this);
    }

    private void OnInteractAlternate(object sender, EventArgs e)
    {
        if (!CanHandleGameplayInput())
            return;

        selectedCounter?.InteractAlternate(this);
    }

    private void HandleInteractions(Vector2 inputVector)
    {
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir.sqrMagnitude > 0.0001f)
        {
            lastInteraction = moveDir.normalized;
        }

        Vector3 rayOrigin = transform.position + Vector3.up * interactionRayHeight;

        if (Physics.Raycast(rayOrigin, lastInteraction, out RaycastHit hitInfo, interactionDistance, counterLayerMask, QueryTriggerInteraction.Ignore))
        {
            BaseCounter baseCounter = hitInfo.transform.GetComponentInParent<BaseCounter>();
            SetSelectedCounter(baseCounter);
            return;
        }

        SetSelectedCounter(null);
    }

    private void HandleMovement(Vector2 inputVector)
    {
        Vector3 inputDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (inputDir.sqrMagnitude < 0.0001f)
        {
            isWalking = false;
            return;
        }

        float inputMagnitude = Mathf.Clamp01(inputDir.magnitude);
        Vector3 moveDir = inputDir.normalized;
        float moveDistance = moveSpeed * inputMagnitude * _clock.DeltaTime;

        bool moved = TryMove(moveDir, moveDistance, out RaycastHit hitInfo);

        if (!moved)
        {
            Vector3 desiredMove = moveDir * moveDistance;
            Vector3 slideMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal);
            slideMove.y = 0f;

            if (slideMove.sqrMagnitude > 0.0001f)
            {
                Vector3 slideDir = slideMove.normalized;
                float slideDistance = slideMove.magnitude;

                moved = TryMove(slideDir, slideDistance, out _);

                if (moved)
                {
                    moveDir = slideDir;
                    moveDistance = slideDistance;
                }
            }
        }

        if (moved)
        {
            transform.position += moveDir * moveDistance;
        }

        Vector3 lookDir = inputDir.normalized;
        transform.forward = Vector3.Slerp(transform.forward, lookDir, _clock.DeltaTime * rotationSpeed);

        isWalking = moved;
    }

    private bool TryMove(Vector3 moveDir, float moveDistance, out RaycastHit hitInfo)
    {
        if (moveDistance <= 0f || moveDir.sqrMagnitude < 0.0001f)
        {
            hitInfo = default;
            return false;
        }

        return !Physics.CapsuleCast(
            GetCapsuleBottom(),
            GetCapsuleTop(),
            playerRadius,
            moveDir,
            out hitInfo,
            moveDistance,
            movementCollisionMask,
            QueryTriggerInteraction.Ignore
        );
    }

    private Vector3 GetCapsuleBottom()
    {
        return transform.position + Vector3.up * playerRadius;
    }

    private Vector3 GetCapsuleTop()
    {
        float topHeight = Mathf.Max(playerRadius, playerHeight - playerRadius);
        return transform.position + Vector3.up * topHeight;
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

    private bool CanHandleGameplayInput()
    {
        if (!_gameService.IsGamePlaying())
            return false;

        if (_pauseService.IsPaused)
            return false;

        return true;
    }
}
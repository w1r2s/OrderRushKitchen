using Assets.Scripts.Cooking;
using Assets.Scripts.Managers.Game;
using Assets.Scripts.Serving;
using System;
using UnityEngine;
using Zenject;

public class StoveCounter : BaseCounter, IHasProgress
{
    private IGameClock _clock;
    private PlateAssemblyService _plateAssemblyService;
    private CookingProcessRecipeResolver _recipesResolver;

    private float fryingTimer;
    private float burningTimer;
    private TimedCookingProcessRecipeSo fryingRecipeSo;
    private TimedCookingProcessRecipeSo burningRecipeSo;

    private State state = State.Idle;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler OnInvalidAction;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [Inject]
    private void Construct(CookingProcessRecipeResolver recipesResolver, PlateAssemblyService plateAssemblyService, IGameClock clock)
    {
        _recipesResolver = recipesResolver;
        _plateAssemblyService = plateAssemblyService;
        _clock = clock;
    }

    private void Update()
    {
        if (!HasObject)
            return;

        switch (state)
        {
            case State.Frying:
                HandleFrying();
                break;
            case State.Fried:
                HandleBurning();
                break;
        }
    }

    public override void Interact(Player player)
    {
        if (!HasObject)
        {
            TryPlaceFryableObject(player);
            return;
        }

        if (!player.HasObject)
        {
            if (TryTransferObjectTo(player))
            {
                ResetCookingState();
            }
            return;
        }

        TryAddCounterObjectToPlate(player);
    }

    private void TryPlaceFryableObject(Player player)
    {
        if (!player.HasObject)
            return;

        var playerObject = player.GetObject();

        if (!_recipesResolver.TryGetSingleInputRecipe<TimedCookingProcessRecipeSo>(CookingProcessType.Frying, playerObject.KitchenObjectSo, out var recipe))
        {
            OnInvalidAction?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (!TryPlaceObjectFromPlayer(player))
            return;

        fryingRecipeSo = recipe;
        fryingTimer = 0f;

        SetState(State.Frying);
        EmitProgress(0f);
    }

    private void TryAddCounterObjectToPlate(Player player)
    {
        if (!player.TryGetObjectAs<PlateKitchenObject>(out var plate))
            return;

        if (!_plateAssemblyService.TryAddIngredientFrom(plate, this))
        {
            OnInvalidAction?.Invoke(this, EventArgs.Empty);
            return;
        }

        ResetCookingState();
    }

    private void HandleFrying()
    {
        if (fryingRecipeSo == null || fryingRecipeSo.Duration <= 0f)
        {
            ResetCookingState();
            return;
        }

        fryingTimer += _clock.DeltaTime;

        if (fryingTimer >= fryingRecipeSo.Duration)
        {
            CompleteFrying();
            return;
        }

        EmitProgress(fryingTimer / fryingRecipeSo.Duration);
    }

    private void CompleteFrying()
    {
        var friedOutput = fryingRecipeSo.OutputKitchenObject;
        if (friedOutput == null || friedOutput.prefab == null)
        {
            if (TryRemoveAndDestroyObject())
            {
                ResetCookingState();
            }
            return;
        }

        if (!TryRemoveAndDestroyObject())
            return;

        if (!TrySpawnAndSet(friedOutput.prefab, out _))
        {
            ResetCookingState();
            return;
        }

        fryingTimer = 0f;
        burningTimer = 0f;

        if (!_recipesResolver.TryGetSingleInputRecipe<TimedCookingProcessRecipeSo>(CookingProcessType.Burning, friedOutput, out var recipe))
        {
            ResetCookingState();
            return;
        }

        burningRecipeSo = recipe;

        SetState(State.Fried);
        EmitProgress(0f);
    }

    private void HandleBurning()
    {
        if (burningRecipeSo == null || burningRecipeSo.Duration <= 0f)
        {
            ResetCookingState();
            return;
        }

        burningTimer += _clock.DeltaTime;

        if (burningTimer >= burningRecipeSo.Duration)
        {
            CompleteBurning();
            return;
        }

        EmitProgress(burningTimer / burningRecipeSo.Duration);
    }

    private void CompleteBurning()
    {
        var burnedOutput = burningRecipeSo.OutputKitchenObject;
        if (burnedOutput == null || burnedOutput.prefab == null)
        {
            if (TryRemoveAndDestroyObject())
            {
                ResetCookingState();
            }
            return;
        }

        if (!TryRemoveAndDestroyObject())
            return;

        if (!TrySpawnAndSet(burnedOutput.prefab, out _))
        {
            ResetCookingState();
            return;
        }

        SetState(State.Burned);
        EmitProgress(0f);
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }

    public override void ResetForLevelTransition()
    {
        base.ResetForLevelTransition();
        ResetCookingState();
    }
    private void ResetCookingState()
    {
        fryingTimer = 0f;
        burningTimer = 0f;
        fryingRecipeSo = null;
        burningRecipeSo = null;

        SetState(State.Idle);
        EmitProgress(0f);
    }

    private void SetState(State newState)
    {
        if (state == newState)
            return;

        state = newState;

        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state
        });
    }

    private void EmitProgress(float progressNormalized)
    {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = Mathf.Clamp01(progressNormalized)
        });
    }
}

using Assets.Scripts;
using System;
using UnityEngine;
using Zenject;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
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

    private RecipeDatabase _recipes;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSo fryingRecipeSo;
    private BurningRecipeSo burningRecipeSo;

    private State state = State.Idle;


    [Inject]
    private void Construct(RecipeDatabase recipes)
    {
        _recipes = recipes;
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
        //положить на стол
        if (!HasObject)
        {
            if (!player.HasObject)
                return;
            var obj = player.GetObject();

            if (!_recipes.TryGetRecipe<FryingRecipeSo>(RecipeType.Frying, obj.KitchenObjectSo, out var recipe))
                return;

            fryingRecipeSo = recipe;
            PlaceObjectFromPlayer(player);

            state = State.Frying;
            fryingTimer = 0;

            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = fryingTimer / fryingRecipeSo.fryingTimerMax
            });
            return;
        }
        //взять со cтола
        if (!player.HasObject)
        {
            var obj = RemoveObject();
            player.SetObject(obj);

            state = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = 0f
            });
            return;
        }

        // попробовать положить на тарелку
        var playerObj = player.GetObject();
        var counterObj = GetObject();
        if (playerObj.TryGetPlate(out var plate))
        {
            if (plate.TryAddIngredient(counterObj.KitchenObjectSo))
            {

                counterObj = RemoveObject();
                Destroy(counterObj.gameObject);

                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }
    private void HandleFrying()
    {
        fryingTimer += Time.deltaTime;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = fryingTimer / fryingRecipeSo.fryingTimerMax
        });

        if (fryingTimer > fryingRecipeSo.fryingTimerMax)
        {
            fryingTimer = 0;
            var obj = RemoveObject();
            Destroy(obj.gameObject);

            SpawnAndSet(fryingRecipeSo.output.prefab);

            state = State.Fried;
            burningTimer = 0;
            if (!_recipes.TryGetRecipe<BurningRecipeSo>(RecipeType.Burning, fryingRecipeSo.output, out var recipe))
            {
                Debug.Log($"Not found Burning recipe for {fryingRecipeSo.output}");
                state = State.Idle;
                return;
            }
            burningRecipeSo = recipe;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
        }
    }
    private void HandleBurning()
    {
        burningTimer += Time.deltaTime;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = burningTimer / burningRecipeSo.burningTimerMax
        });

        if (burningTimer > burningRecipeSo.burningTimerMax)
        {
            burningTimer = 0;

            var obj = RemoveObject();
            Destroy(obj.gameObject);

            SpawnAndSet(burningRecipeSo.output.prefab);

            state = State.Burned;

            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = 0f
            });
        }
    }
    public bool IsFried()
    {
        return state == State.Fried;
    }
}
using Assets.Scripts.Composition;
using Assets.Scripts.Cooking;
using Assets.Scripts.Managers.Game;
using Assets.Scripts.Serving;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Counters
{
    public class PotCounter : BaseCounter, IHasProgress, IIngredientCompositionSource
    {
        public class OnStateChangedEventArgs : EventArgs
        {
            public State State { get; }

            public OnStateChangedEventArgs(State state)
            {
                State = state;
            }
        }

        public enum State
        {
            Idle,
            Completing,
            Cooking,
            Cooked
        }

        private IGameClock _clock;
        private IServedMenuItemFactory _menuItemFactory;
        private CookingProcessRecipeResolver _recipesResolver;

        private List<KitchenObjectSo> currentIngredients;
        private TimedCookingProcessRecipeSo _recipe;

        private State state = State.Idle;
        private float cookingTimer;

        public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
        public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
        public event EventHandler OnIngredientsChanged;
        public event EventHandler OnIngredientAdded;
        public event EventHandler OnCleared;
        public event EventHandler OnInvalidAction;

        public IReadOnlyList<KitchenObjectSo> Ingredients => currentIngredients;

        [Inject]
        private void Construct(CookingProcessRecipeResolver recipesResolver, IServedMenuItemFactory menuItemFactory, IGameClock clock)
        {
            _recipesResolver = recipesResolver;
            _menuItemFactory = menuItemFactory;
            _clock = clock;

            currentIngredients = new List<KitchenObjectSo>();
        }

        private void Update()
        {
            if (state == State.Cooking)
            {
                HandleCooking();
            }
        }

        public override void Interact(Player player)
        {
            if (state == State.Idle || state == State.Completing)
            {
                TryAddIngredient(player);
                return;
            }

            if (state == State.Cooked)
            {
                TryTakeCookedItem(player);
            }
        }

        public override bool TryInteractAlternate(Player player)
        {
            if (state == State.Idle)
                return false;

            ResetCookingState();
            OnCleared?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public override void ResetForLevelTransition()
        {
            base.ResetForLevelTransition();
            ResetCookingState();
        }

        public IReadOnlyList<KitchenObjectSo> GetIngredients()
        {
            return currentIngredients;
        }

        private void TryAddIngredient(Player player)
        {
            if (!player.HasObject)
                return;

            KitchenObject playerObject = player.GetObject();
            KitchenObjectSo ingredient = playerObject.KitchenObjectSo;

            if (!_recipesResolver.CanAddInput(
                    CookingProcessType.PotCooking,
                    ingredient,
                    currentIngredients))
            {
                OnInvalidAction?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (!player.TryRemoveAndDestroyObject())
                return;

            currentIngredients.Add(ingredient);

            OnIngredientAdded?.Invoke(this, EventArgs.Empty);
            OnIngredientsChanged?.Invoke(this, EventArgs.Empty);

            if (_recipesResolver.TryResolveExact(
                    CookingProcessType.PotCooking,
                    currentIngredients,
                    out _recipe))
            {
                cookingTimer = 0f;
                SetState(State.Cooking);
                EmitProgress(0f);
                return;
            }

            SetState(State.Completing);
        }

        private void TryTakeCookedItem(Player player)
        {
            if (player.HasObject)
                return;

            if (_recipe == null || _recipe.OutputMenuItem == null)
                return;

            if (_menuItemFactory.TryCreate(_recipe.OutputMenuItem, player, out _))
            {
                ResetCookingState();
            }
        }

        private void HandleCooking()
        {
            if (_recipe == null || _recipe.Duration <= 0f)
            {
                ResetCookingState();
                return;
            }

            cookingTimer += _clock.DeltaTime;

            if (cookingTimer >= _recipe.Duration)
            {
                CompleteCooking();
                return;
            }

            EmitProgress(cookingTimer / _recipe.Duration);
        }

        private void CompleteCooking()
        {
            var cookedOutput = _recipe.OutputMenuItem;

            if (cookedOutput == null || cookedOutput.servedVisualPrefab == null)
            {
                ResetCookingState();
                return;
            }

            SetState(State.Cooked);
            EmitProgress(0f);
        }

        private void ResetCookingState()
        {
            cookingTimer = 0f;
            _recipe = null;
            currentIngredients.Clear();

            SetState(State.Idle);
            OnIngredientsChanged?.Invoke(this, EventArgs.Empty);
            EmitProgress(0f);
        }

        private void SetState(State newState)
        {
            if (state == newState)
                return;

            state = newState;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs(state));
        }

        private void EmitProgress(float progressNormalized)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = Mathf.Clamp01(progressNormalized)
            });
        }
    }
}
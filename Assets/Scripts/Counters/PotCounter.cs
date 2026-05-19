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
        public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
        public event EventHandler OnIngredientsChanged;
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

        public IReadOnlyList<KitchenObjectSo> Ingredients => currentIngredients;

        [Inject]
        private void Construct(CookingProcessRecipeResolver recipesResolver, IServedMenuItemFactory menuItemFactory, IGameClock clock)
        {
            _recipesResolver = recipesResolver;
            _menuItemFactory = menuItemFactory;
            _clock = clock;
            currentIngredients = new();
        }
        private void Update()
        {
            switch (state)
            {
                case State.Cooking:
                    HandleCooking();
                    break;
                default:
                    break;
            }
        }

        public override void Interact(Player player)
        {
            if (state == State.Idle || state == State.Completing)
            {
                if (!player.HasObject)
                    return;

                var playerObjSo = player.GetObject().KitchenObjectSo;

                if (!_recipesResolver.CanAddInput(CookingProcessType.PotCooking, playerObjSo, currentIngredients))
                    return;
                currentIngredients.Add(playerObjSo);
               OnIngredientsChanged?.Invoke(this, EventArgs.Empty);

                RemoveAndDestroy(player);

                if (_recipesResolver.TryResolveExact(CookingProcessType.PotCooking, currentIngredients, out _recipe))
                {
                    cookingTimer = 0;
                    state = State.Cooking;
                }
                else
                {
                    state = State.Completing;
                }
            }
            else if (state == State.Cooked)
            {
                if (player.HasObject)
                    return;

                HandleCooked(player);
            }

        }

        private void HandleCooked(Player player)
        {
            if (_recipe == null || _recipe.OutputMenuItem == null)
                return;

            if (_menuItemFactory.TryCreate(_recipe.OutputMenuItem, player, out _))
            {
                ResetCookingState();
            }
        }

        private void RemoveAndDestroy(ObjectHolder holder = null)
        {
            KitchenObject obj = null;
            if (holder != null)
            {
                obj = holder.RemoveObject();
            }
            else
            {
                obj = RemoveObject();
            }

            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }

        public override void InteractAlternate(Player player)
        {
            if (state == State.Idle)
                return;

            ResetCookingState();
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
            var cookedOut = _recipe.OutputMenuItem;
            if (cookedOut == null || cookedOut.servedVisualPrefab == null)
            {
                RemoveAndDestroy();
                ResetCookingState();
                return;
            }
            // UI\audio completion effect

            state = State.Cooked;
            EmitProgress(0f);
        }

        public override void ResetForLevelTransition()
        {
            base.ResetForLevelTransition();
            ResetCookingState();
        }
        private void ResetCookingState()
        {
            cookingTimer = 0f;
            _recipe = null;
            currentIngredients.Clear();
            state = State.Idle;

            OnIngredientsChanged?.Invoke(this, EventArgs.Empty);
            EmitProgress(0f);

        }

        private void EmitProgress(float progressNormalized)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = Mathf.Clamp01(progressNormalized)
            });
        }

        public IReadOnlyList<KitchenObjectSo> GetIngredients()
        {
            return currentIngredients;
        }
    }
}

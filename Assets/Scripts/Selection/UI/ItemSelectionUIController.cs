using System;
using Zenject;

namespace Assets.Scripts.Selection
{
    public class ItemSelectionUIController : IInitializable, IDisposable
    {
        private readonly IItemSelectionService _selectionService;
        private readonly ItemSelectionUI _selectionUI;

        [Inject]
        public ItemSelectionUIController(IItemSelectionService selectionService, ItemSelectionUI selectionUI)
        {
            _selectionService = selectionService;
            _selectionUI = selectionUI;
        }

        public void Initialize()
        {
            _selectionUI.InitializeHidden();

            _selectionUI.CloseRequested += SelectionUI_CloseRequested;
            _selectionUI.IngredientSelected += SelectionUI_IngredientSelected;
            _selectionUI.DrinkSelected += SelectionUI_DrinkSelected;

            _selectionService.OnSelectionOpened += SelectionService_OnSelectionOpened;
            _selectionService.OnSelectionClosed += SelectionService_OnSelectionClosed;

            if (_selectionService.IsOpen)
                ShowCurrentSelection();
        }

        public void Dispose()
        {
            _selectionUI.CloseRequested -= SelectionUI_CloseRequested;
            _selectionUI.IngredientSelected -= SelectionUI_IngredientSelected;
            _selectionUI.DrinkSelected -= SelectionUI_DrinkSelected;

            _selectionService.OnSelectionOpened -= SelectionService_OnSelectionOpened;
            _selectionService.OnSelectionClosed -= SelectionService_OnSelectionClosed;
        }

        private void SelectionService_OnSelectionOpened(object sender, EventArgs e)
        {
            ShowCurrentSelection();
        }

        private void SelectionService_OnSelectionClosed(object sender, EventArgs e)
        {
            _selectionUI.Hide();
        }

        private void SelectionUI_CloseRequested(object sender, EventArgs e)
        {
            _selectionService.CloseSelection();
        }

        private void SelectionUI_IngredientSelected(object sender, ItemSelectionIngredientSelectedEventArgs e)
        {
            if (e?.Ingredient == null)
                return;

            _selectionService.TryConfirmSelection(e.Ingredient);
        }

        private void SelectionUI_DrinkSelected(object sender, ItemSelectionDrinkSelectedEventArgs e)
        {
            if (e?.Drink == null)
                return;

            _selectionService.TryConfirmSelection(e.Drink);
        }

        private void ShowCurrentSelection()
        {
            switch (_selectionService.CurrentMode)
            {
                case ItemSelectionMode.Ingredients:
                    _selectionUI.ShowIngredients(_selectionService.CurrentIngredientOptions);
                    break;

                case ItemSelectionMode.Drinks:
                    _selectionUI.ShowDrinks(_selectionService.CurrentDrinkOptions);
                    break;

                default:
                    _selectionUI.Hide();
                    break;
            }
        }
    }
}
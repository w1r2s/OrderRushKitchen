using OrderRushKitchen.Selection;
using System;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Input
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MobileGameplayControlsUI : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        private IItemSelectionService _selectionService;

        [Inject]
        private void Construct(IItemSelectionService selectionService)
        {
            _selectionService = selectionService;
        }
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        private void Start()
        {
            _selectionService.OnSelectionOpened += SelectionService_OnSelectionOpened;
            _selectionService.OnSelectionClosed += SelectionService_OnSelectionClosed;
            RefreshVisibility();
        }
        private void OnDestroy()
        {
            if (_selectionService != null)
            {
                _selectionService.OnSelectionOpened -= SelectionService_OnSelectionOpened;
                _selectionService.OnSelectionClosed -= SelectionService_OnSelectionClosed;
            }
        }

        private void SelectionService_OnSelectionClosed(object sender, EventArgs e)
        {
            RefreshVisibility();
        }

        private void SelectionService_OnSelectionOpened(object sender, EventArgs e)
        {
            RefreshVisibility();
        }

        private void RefreshVisibility()
        {
            if (_selectionService.IsOpen)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
        private void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
        }
        private void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }
    }
}

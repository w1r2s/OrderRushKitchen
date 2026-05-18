using System;
using UnityEngine;

namespace Assets.Scripts.Managers.Input
{
    public interface IInputService
    {
        event EventHandler OnInteractAction;
        event EventHandler OnInteractAlternateAction;
        event EventHandler OnPauseAction;

        Vector2 GetMovementVector();

        string GetKeyBindingText(InputKeyBinding binding);

        void RebindKeyBinding(InputKeyBinding binding, Action onActionRebound);
    }
}

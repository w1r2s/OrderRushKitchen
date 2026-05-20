using System;
using UnityEngine;

namespace Assets.Scripts.Managers.Input
{
    public interface IGameplayInputService
    {
        event EventHandler OnInteractAction;
        event EventHandler OnInteractAlternateAction;
        event EventHandler OnPauseAction;

        Vector2 GetMovementVector();
    }
}

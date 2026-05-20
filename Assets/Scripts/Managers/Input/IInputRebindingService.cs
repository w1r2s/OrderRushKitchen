using System;

namespace Assets.Scripts.Managers.Input
{
    public interface IInputRebindingService
    {
        string GetKeyBindingText(InputKeyBinding binding);
        void RebindKeyBinding(InputKeyBinding binding, Action<bool> onFinished);
    }
}

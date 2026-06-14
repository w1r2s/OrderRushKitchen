using System;

namespace OrderRushKitchen.Input
{
    public interface IInputRebindingService
    {
        string GetKeyBindingText(InputKeyBinding binding);
        void RebindKeyBinding(InputKeyBinding binding, Action<bool> onFinished);
    }
}

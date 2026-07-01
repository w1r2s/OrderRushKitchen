using System;

namespace OrderRushKitchen.Sdk
{
    public interface ISdkInitializationService
    {
        SdkInitializationState State { get; }
        bool IsReady { get; }
        bool IsDisabled { get; }
        string ErrorMessage { get; }

        event Action<SdkInitializationState> StateChanged;

        void Initialize();
    }
}
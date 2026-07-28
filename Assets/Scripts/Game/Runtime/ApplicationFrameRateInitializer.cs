using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Game
{
    public sealed class ApplicationFrameRateInitializer : IInitializable
    {
        private const int TARGET_FRAME_RATE = 60;

        public void Initialize()
        {
            Application.targetFrameRate = TARGET_FRAME_RATE;
        }
    }
}

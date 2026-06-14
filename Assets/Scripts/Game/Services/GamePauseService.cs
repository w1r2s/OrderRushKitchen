using System.Collections.Generic;
using System;
using UnityEngine;

namespace OrderRushKitchen.Game
{
    public class GamePauseService : IGamePauseService, IGameClock
    {
        public bool IsPaused => _reasonCounts.Count > 0;
        public float DeltaTime => IsPaused ? 0f : Time.deltaTime;

        public event EventHandler OnPauseChanged;

        private readonly Dictionary<GamePauseReason, int> _reasonCounts = new();
        public void AddPause(GamePauseReason reason)
        {

            if (_reasonCounts.TryGetValue(reason, out var value))
                _reasonCounts[reason] = value + 1;
            else
                _reasonCounts.Add(reason, 1);

            OnPauseChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RemovePause(GamePauseReason reason)
        {
            if (_reasonCounts.TryGetValue(reason, out var value))
            {
                var newValue = value - 1;
                if (newValue <= 0)
                    _reasonCounts.Remove(reason);
                else
                    _reasonCounts[reason] = newValue;

                OnPauseChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public void ToggleUserPause()
        {
            if (HasPause(GamePauseReason.UserPause))
                RemovePause(GamePauseReason.UserPause);
            else
                AddPause(GamePauseReason.UserPause);
        }
        public bool HasPause(GamePauseReason reason)
        {
            return _reasonCounts.ContainsKey(reason);
        }
    }
}
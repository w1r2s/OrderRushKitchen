namespace OrderRushKitchen.UserProgress
{
    public class UserProgressData
    {
        public int CurrentLevelNumber { get; private set; }
        public int MaxUnlockedLevelNumber { get; private set; }
        public bool HasSeenHowToPlay { get; private set; }

        public UserProgressData(int currentLevelNumber, int maxUnlockedLevelNumber, bool hasSeenHowToPlay)
        {
            CurrentLevelNumber = currentLevelNumber;
            MaxUnlockedLevelNumber = maxUnlockedLevelNumber;
            HasSeenHowToPlay = hasSeenHowToPlay;
        }
    }
}

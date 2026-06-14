namespace OrderRushKitchen.UserProgress
{
    public class UserProgressData
    {
        public int CurrentLevelNumber { get; private set; }
        public int MaxUnlockedLevelNumber { get; private set; }

        public UserProgressData(int currentLevelNumber, int maxUnlockedLevelNumber)
        {
            CurrentLevelNumber = currentLevelNumber;
            MaxUnlockedLevelNumber = maxUnlockedLevelNumber;
        }
    }
}

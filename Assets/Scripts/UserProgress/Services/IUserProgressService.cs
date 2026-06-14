namespace OrderRushKitchen.UserProgress
{
    public interface IUserProgressService
    {
         int CurrentLevel { get; }
         int UnlockedLevelNumber { get; }

        void SetCurrentLevel(int level);
        void UnlockLevel(int level);
        void CompleteLevel(int level);
    }
}

namespace OrderRushKitchen.UserProgress
{
    public interface IUserProgressService
    {
         int CurrentLevel { get; }
         int UnlockedLevelNumber { get; }
         bool HasSeenHowToPlay { get; }

        void SetCurrentLevel(int level);
        void UnlockLevel(int level);
        void CompleteLevel(int level);
        void MarkHowToPlaySeen();
    }
}

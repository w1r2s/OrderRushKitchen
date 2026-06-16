using Zenject;

namespace OrderRushKitchen.UserProgress
{
    public class UserProgressService : IUserProgressService, IInitializable
    {
        private readonly IUserProgressStorage _userProgressStorage;
        public int CurrentLevel => _userProgressData.CurrentLevelNumber;

        public int UnlockedLevelNumber => _userProgressData.MaxUnlockedLevelNumber;

        private UserProgressData _userProgressData;

        [Inject]
        public UserProgressService(IUserProgressStorage userProgressStorage)
        {
            _userProgressStorage = userProgressStorage;
        }

        public void Initialize()
        {
            _userProgressData = _userProgressStorage.Load();
        }

        public void CompleteLevel(int completedLevelNumber)
        {
            if (completedLevelNumber <= 0)
                return;

            UnlockLevel(completedLevelNumber + 1);
        }

        public void SetCurrentLevel(int level)
        {
            if (level <= 0)
                return;

            if (level == CurrentLevel)
                return;

            if (level > UnlockedLevelNumber)
                level = UnlockedLevelNumber;

            _userProgressData = new UserProgressData(level, UnlockedLevelNumber);
            _userProgressStorage.Save(_userProgressData);
        }

        public void UnlockLevel(int levelNumber)
        {
            if (levelNumber <= 0)
                return;

            if (levelNumber <= UnlockedLevelNumber)
                return;

            _userProgressData = new UserProgressData(CurrentLevel, levelNumber);
            _userProgressStorage.Save(_userProgressData);
        }
    }
}

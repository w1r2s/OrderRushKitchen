namespace Assets.Scripts.Progress
{
    public interface IUserProgressStorage
    {
        UserProgressData Load();
        void Save(UserProgressData data);
    }
}

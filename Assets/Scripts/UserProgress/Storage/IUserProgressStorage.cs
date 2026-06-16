namespace OrderRushKitchen.UserProgress
{
    public interface IUserProgressStorage
    {
        UserProgressData Load();
        void Save(UserProgressData data);
    }
}

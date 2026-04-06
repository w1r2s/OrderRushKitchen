namespace Assets.Scripts.Managers.Input
{
    public interface IInputStorage
    {
        void SaveBindings(string json);
        string LoadBindings();
    }
}

namespace OrderRushKitchen.Input
{
    public interface IInputStorage
    {
        void SaveBindings(string json);
        string LoadBindings();
    }
}

namespace OrderRushKitchen.Navigation
{
    public interface ILoadingScreen
    {
        void Show();
        void Hide();
        void SetProgress(float progress);
    }
}
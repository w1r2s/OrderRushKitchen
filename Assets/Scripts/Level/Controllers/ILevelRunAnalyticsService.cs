namespace OrderRushKitchen.Level
{
    public interface ILevelRunAnalyticsService
    {
        void LogLevelRetryRequested();
        void LogLevelReturnToMenuRequested();
    }
}
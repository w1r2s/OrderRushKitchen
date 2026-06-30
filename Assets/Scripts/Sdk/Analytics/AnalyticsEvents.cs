namespace OrderRushKitchen.Sdk
{
    public static class AnalyticsEvents
    {
        public const string AppStart = "app_start";
        public const string FirebaseInitSucceeded = "firebase_init_succeeded";
        public const string FirebaseInitFailed = "firebase_init_failed";

        public const string LevelStarted = "level_started";
        public const string LevelCompleted = "level_completed";
        public const string LevelFailed = "level_failed";
        public const string LevelRetried = "level_retried";
        public const string LevelReturnedToMenu = "level_returned_to_menu";

        public const string OrderCompleted = "order_completed";
        public const string OrderFailed = "order_failed";
    }
}
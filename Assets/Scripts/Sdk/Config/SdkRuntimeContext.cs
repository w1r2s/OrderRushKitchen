using UnityEngine;

namespace OrderRushKitchen.Sdk
{
    public static class SdkRuntimeContext
    {
        public static string GetEnvironmentName(SdkSettings settings)
        {
            return settings != null
                ? settings.environment.ToString().ToLowerInvariant()
                : "unknown";
        }

        public static string GetPlatformName()
        {
            return Application.platform.ToString().ToLowerInvariant();
        }

        public static string GetAppVersion()
        {
            return Application.version;
        }

        public static string GetBuildType()
        {
#if UNITY_EDITOR
            return "editor";
#else
            return Debug.isDebugBuild ? "development_build" : "release";
#endif
        }
    }
}

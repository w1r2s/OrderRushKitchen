using UnityEngine;

namespace OrderRushKitchen.Sdk
{
    [CreateAssetMenu(fileName = "SdkSettings", menuName = "Order Rush Kitchen/Sdk/Sdk Settings")]
    public class SdkSettings : ScriptableObject
    {
        [Header("General")]
        public bool sdkEnabled = true;
        public SdkEnvironment environment = SdkEnvironment.Development;
        public bool verboseSdkLogging = true;

        [Header("Firebase")]
        public bool firebaseEnabled = true;
        public bool analyticsEnabled = true;
        public bool crashlyticsEnabled = true;
    }
}
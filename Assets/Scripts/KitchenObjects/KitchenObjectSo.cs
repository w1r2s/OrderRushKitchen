using UnityEngine;

namespace OrderRushKitchen.KitchenObjects
{

    [CreateAssetMenu()]
    public class KitchenObjectSo : ScriptableObject
    {
        public KitchenObject prefab;
        public Sprite sprite;
        public string objectName;
    }

}

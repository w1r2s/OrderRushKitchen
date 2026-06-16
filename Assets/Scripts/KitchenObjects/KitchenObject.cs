using UnityEngine;

namespace OrderRushKitchen.KitchenObjects
{

    public class KitchenObject : MonoBehaviour
    {
        [SerializeField] private KitchenObjectSo kitchenObjectSo;

        public KitchenObjectSo KitchenObjectSo => kitchenObjectSo;

        private ObjectHolder _holder;

        public ObjectHolder Holder => _holder;

        public void SetHolder(ObjectHolder holder)
        {
            _holder = holder;
        }

        public void ClearHolder()
        {
            _holder = null;
        }
    }
}

using Assets.Scripts.Serving;
using UnityEngine;

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

    public bool TryGetPlate(out PlateKitchenObject plate)
    {
        if (this is PlateKitchenObject p)
        {
            plate = p;
            return true;
        }

        plate = null;
        return false;
    }
}
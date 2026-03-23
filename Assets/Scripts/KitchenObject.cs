using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSo kitchenObjectSo;

    private ClearCounter clearCounter;
    public KitchenObjectSo GetKitchenObjectSo()
    {
        return kitchenObjectSo;
    }

    public void SetClearCounter(ClearCounter clearCounter)
    {
        if (this.clearCounter != null)
        {
            this.clearCounter.ClearKitchenObject();
        }

        this.clearCounter = clearCounter;

        if (clearCounter.HasKitchenObject())
        {
            Debug.LogError("׃זו וסעü מבתוךע");
            return;
        }

        clearCounter.SetKitchenObjcet(this);

        transform.parent = clearCounter.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public ClearCounter GetClearCounter()
    {
        return clearCounter;
    }
}

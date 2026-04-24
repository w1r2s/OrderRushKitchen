using UnityEngine;

namespace Assets.Scripts.Serving
{
    public class PlateIconsUI : MonoBehaviour
    {
        [SerializeField] private PlateKitchenObject plateKitchenObject;
        [SerializeField] private Transform iconTemplate;
        private void Awake()
        {
            iconTemplate.gameObject.SetActive(false);
        }
        private void Start()
        {
            plateKitchenObject.OnIngredientsChanged += PlateKitchenObject_OnIngredientsChanged;
            UpdateVisual();
        }
        private void OnDestroy()
        {
            plateKitchenObject.OnIngredientsChanged -= PlateKitchenObject_OnIngredientsChanged;
        }

        private void PlateKitchenObject_OnIngredientsChanged(object sender, IngredientsChangedEventArgs e)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (plateKitchenObject.State != PlateState.Assembly)
                return;

            foreach (Transform child in transform)
            {
                if (child == iconTemplate)
                    continue;
                Destroy(child.gameObject);
            }
            foreach (var kitchenObjectSo in plateKitchenObject.GetKitchenObjectSoList())
            {

                Transform iconTransform = Instantiate(iconTemplate, transform);
                iconTransform.gameObject.SetActive(true);
                iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSo(kitchenObjectSo);
            }
        }
    }
}

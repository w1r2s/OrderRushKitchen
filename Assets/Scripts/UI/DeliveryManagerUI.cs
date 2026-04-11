using Assets.Scripts;
using UnityEngine;
using Zenject;

public class DeliveryManagerUI : MonoBehaviour
{
    private IDeliveryService _deliveryService;
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);

    }

    [Inject]
    private void Construct(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }
    private void Start()
    {
        _deliveryService.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        _deliveryService.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;

        UpdateVisual();
    }
    private void OnDestroy()
    {
        if (_deliveryService == null) return;

        _deliveryService.OnRecipeSpawned -= DeliveryManager_OnRecipeSpawned;
        _deliveryService.OnRecipeCompleted -= DeliveryManager_OnRecipeCompleted;
    }
    private void DeliveryManager_OnRecipeSpawned(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }
    private void DeliveryManager_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == recipeTemplate)
                continue;
            Destroy(child.gameObject);
        }
        foreach (var recipeSo in _deliveryService.GetWaitingRecipes())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSo(recipeSo);
        }
    }
}

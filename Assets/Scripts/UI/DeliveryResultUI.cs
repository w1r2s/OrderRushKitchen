using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DeliveryResultUI : MonoBehaviour
{
    private IDeliveryService _deliveryService;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;

    [SerializeField] private Color successColor;
    [SerializeField] private Color failedColor;

    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failedSprite;


    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    [Inject]
    private void Construct(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }
    private void Start()
    {
        _deliveryService.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        _deliveryService.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        gameObject.SetActive(false);

    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger("Popup");

        backgroundImage.color = failedColor;
        iconImage.sprite = failedSprite;
        messageText.text = "DELIVERY\nFAILED";

       

    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger("Popup");

        backgroundImage.color = successColor;
        iconImage.sprite = successSprite;
        messageText.text = "DELIVERY\nSUCCESS";

       
    }
}

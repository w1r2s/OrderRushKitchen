using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button altInteractButton;
    [SerializeField] private Button pauseButton;

    [SerializeField] private TextMeshProUGUI soundEffectText;
    [SerializeField] private TextMeshProUGUI musicText;

    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI altInteractText;
    [SerializeField] private TextMeshProUGUI pauseText;

    [SerializeField] private Transform pressToRebindKeyTransform;


    private void Awake()
    {
        Instance = this;

        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });

        moveUpButton.onClick.AddListener(() =>
        {
            RebindBinding(InputManager.KeyBinding.Move_Up);
        });
        moveDownButton.onClick.AddListener(() =>
        {
            RebindBinding(InputManager.KeyBinding.Move_Down);
        });
        moveLeftButton.onClick.AddListener(() =>
        {
            RebindBinding(InputManager.KeyBinding.Move_Left);
        });
        moveRightButton.onClick.AddListener(() =>
        {
            RebindBinding(InputManager.KeyBinding.Move_Right);
        });
        interactButton.onClick.AddListener(() =>
        {
            RebindBinding(InputManager.KeyBinding.Interact);
        });
        altInteractButton.onClick.AddListener(() =>
        {
            RebindBinding(InputManager.KeyBinding.Alt_Interact);
        });
        pauseButton.onClick.AddListener(() =>
        {
            RebindBinding(InputManager.KeyBinding.Pause);
        });
    }
    private void Start()
    {
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        UpdateVisual();
        Hide();
        HidePressToRebindKey();
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectText.text = $"Sound Effects: {Mathf.Round(SoundManager.Instance.GetVolume() * 10f)}";
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        moveUpText.text = InputManager.Instance.GetKeyBindingText(InputManager.KeyBinding.Move_Up);
        moveDownText.text = InputManager.Instance.GetKeyBindingText(InputManager.KeyBinding.Move_Down);
        moveLeftText.text = InputManager.Instance.GetKeyBindingText(InputManager.KeyBinding.Move_Left);
        moveRightText.text = InputManager.Instance.GetKeyBindingText(InputManager.KeyBinding.Move_Right);
        interactText.text = InputManager.Instance.GetKeyBindingText(InputManager.KeyBinding.Interact);
        altInteractText.text = InputManager.Instance.GetKeyBindingText(InputManager.KeyBinding.Alt_Interact);
        pauseText.text = InputManager.Instance.GetKeyBindingText(InputManager.KeyBinding.Pause);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }
    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }
    private void RebindBinding(InputManager.KeyBinding binding)
    {
        ShowPressToRebindKey();
        InputManager.Instance.RebindKeyBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}

using Assets.Scripts.Navigation;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuUI : MonoBehaviour
{
    private INavigationService _navigationService;

    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    [Inject]
    private void Construct(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }
    private void Start()
    {
        playButton.onClick.AddListener(PlayClick);
        quitButton.onClick.AddListener(QuitClick);
    }
    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(PlayClick);
        quitButton.onClick.RemoveListener(QuitClick);
    }
    private void PlayClick()
    {
        _navigationService.LoadGameAsync(CancellationToken.None).Forget(Debug.LogException);
    }
    private void QuitClick()
    {
        Application.Quit();
    }
}

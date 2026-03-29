using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayClick);
        quitButton.onClick.AddListener(QuitClick);
    }

    private void PlayClick()
    {
        LoadingManager.Load(LoadingManager.Scene.GameScene);
    }
    private void QuitClick()
    {
        Application.Quit();
    }
}

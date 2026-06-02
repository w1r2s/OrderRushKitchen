using Assets.Scripts.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameLabel;
    [SerializeField] private LevelSelectionUI levelSelectionUI;
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;


    private void Start()
    {
        playButton.onClick.AddListener(PlayClick);
        quitButton.onClick.AddListener(QuitClick);

        levelSelectionUI.Closed += LevelSelectionUI_Closed;
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(PlayClick);
        quitButton.onClick.RemoveListener(QuitClick);

        if (levelSelectionUI != null)
            levelSelectionUI.Closed -= LevelSelectionUI_Closed;
    }

    private void PlayClick()
    {
        SetMainMenuVisible(false);
        levelSelectionUI.Show();
    }

    private void LevelSelectionUI_Closed(object sender, EventArgs e)
    {
        SetMainMenuVisible(true);
    }

    private void SetMainMenuVisible(bool visible)
    {
        mainMenuPanel.SetActive(visible);
        gameLabel.SetActive(visible);
    }
    private void QuitClick()
    {
        Application.Quit();
    }
}

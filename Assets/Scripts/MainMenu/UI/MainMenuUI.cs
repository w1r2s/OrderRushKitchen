using OrderRushKitchen.Game;
using OrderRushKitchen.Settings;
using System;
using UnityEngine.UI;
using UnityEngine;

namespace OrderRushKitchen.MainMenu
{

    public class MainMenuUI : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject gameLabel;
        [SerializeField] private LevelSelectionUI levelSelectionUI;
        [SerializeField] private OptionsUI optionsUI;
        [Header("Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button optionsButton;


        private void Start()
        {
            playButton.onClick.AddListener(PlayClick);
            quitButton.onClick.AddListener(QuitClick);
            optionsButton.onClick.AddListener(OptionsClick);

            levelSelectionUI.Closed += LevelSelectionUI_Closed;
            optionsUI.Closed += OptionsUI_Closed;
        }


        private void OnDestroy()
        {
            playButton.onClick.RemoveListener(PlayClick);
            quitButton.onClick.RemoveListener(QuitClick);
            optionsButton.onClick.RemoveListener(OptionsClick);

            if (levelSelectionUI != null)
                levelSelectionUI.Closed -= LevelSelectionUI_Closed;

            if (optionsUI != null)
                optionsUI.Closed -= OptionsUI_Closed;
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

        private void OptionsClick()
        {
            SetMainMenuVisible(false);
            optionsUI.Show();
        }

        private void OptionsUI_Closed(object sender, EventArgs e)
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

}

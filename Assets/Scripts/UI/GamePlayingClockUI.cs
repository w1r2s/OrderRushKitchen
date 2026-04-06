using Assets.Scripts.Managers.Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;
    private IGameService _gameService;

    [Inject]
    private void Construct(IGameService gameService)
    {
        _gameService = gameService;
    }
    private void Update()
    {
        timerImage.fillAmount = _gameService.GetGamePlayingTimerNormalized();
    }
}

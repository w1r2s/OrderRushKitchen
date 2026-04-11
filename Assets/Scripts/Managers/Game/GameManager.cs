using Assets.Scripts.Managers.Game;
using Assets.Scripts.Managers.Input;
using System;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private IGameService _gameService;
    private IInputService _inputManager;

    [Inject]
    private void Construct(IGameService gameService, IInputService inputManager)
    {
        _gameService = gameService;
        _inputManager = inputManager;
    }

    private void Start()
    {
        _inputManager.OnPauseAction += OnPause;
    }
    private void OnDestroy()
    {
        if( _inputManager != null )
        {
            _inputManager.OnPauseAction -= OnPause;
        }
    }
    private void Update()
    {
        _gameService.Tick(Time.deltaTime);
    }
    private void OnPause(object sender, EventArgs e)
    {
        _gameService.TogglePauseGame();

    }
}

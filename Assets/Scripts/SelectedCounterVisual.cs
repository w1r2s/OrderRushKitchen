using UnityEngine;
using Zenject;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private Player _player;

    [Inject]
    public void Construct(Player player)
    {
        _player = player;
    }
    private void Start()
    {
        _player.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }
    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnSelectedCounterChanged -= Player_OnSelectedCounterChanged;
        }
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == baseCounter)
            Show();
        else
            Hide();

    }
    private void Show()
    {
        foreach (var item in visualGameObjectArray)
        {
            item.SetActive(true);
        }

    }
    private void Hide()
    {
        foreach (var item in visualGameObjectArray)
        {
            item.SetActive(false);
        }
    }
}

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
        _player.OnSelectedInteractableChanged += Player_OnSelectedInteractableChanged;
    }
    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnSelectedInteractableChanged -= Player_OnSelectedInteractableChanged;
        }
    }

    private void Player_OnSelectedInteractableChanged(object sender, Player.SelectedInteractableChangedEventArgs e)
    {
        bool isSelected = baseCounter != null && ReferenceEquals(e.SelectedInteractable, baseCounter);

        SetVisible(isSelected);

    }
    private void SetVisible(bool isVisible)
    {
        foreach (GameObject visual in visualGameObjectArray)
        {
            if (visual != null)
            {
                visual.SetActive(isVisible);
            }
        }
    }
}

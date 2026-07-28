using UnityEngine;

namespace OrderRushKitchen.Game
{
    public class GameplayHudUI : MonoBehaviour
    {
        [SerializeField] private Canvas[] hudCanvases;

        public void SetVisible(bool isVisible)
        {
            if (hudCanvases == null)
                return;

            for (int i = 0; i < hudCanvases.Length; i++)
            {
                if (hudCanvases[i] != null)
                    hudCanvases[i].enabled = isVisible;
            }
        }
    }
}

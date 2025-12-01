using UnityEngine;

namespace ClearSky
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private GameObject deathPanel;

        private void Start()
        {
            deathPanel.SetActive(false);
        }

        public void ShowDeathScreen()
        {
            deathPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        public void Retry()
        {
            Time.timeScale = 1f;

            PlayerRespawn player = FindObjectOfType<PlayerRespawn>();
            player.RespawnPlayer();

            deathPanel.SetActive(false);
        }
    }
}

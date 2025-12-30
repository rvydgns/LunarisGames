using UnityEngine;
using UnityEngine.UI;

namespace ClearSky
{
    public class GameUI : MonoBehaviour
    {
        [Header("Health UI")]
        public Image healthFill; 

        [Header("Death Screen")]
        public GameObject deathScreen;

        private void Start()
        {
            if (deathScreen != null)
                deathScreen.SetActive(false);
        }

        
        public void UpdateHealth(int current, int max)
        {
            if (healthFill == null) return;

            healthFill.fillAmount = (float)current / max;
        }

        public void ShowDeathScreen()
        {
            if (deathScreen != null)
                deathScreen.SetActive(true);
        }

        public void HideDeathScreen()
        {
            if (deathScreen != null)
                deathScreen.SetActive(false);
        }
    }
}

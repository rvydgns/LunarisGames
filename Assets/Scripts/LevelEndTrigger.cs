using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndTrigger : MonoBehaviour
{
    public int nextLevelIndex = 2; // Örn: Level1 bittiğinde Level2 açılsın

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("UnlockedLevel", nextLevelIndex);
            SceneManager.LoadScene("MapScene");
        }
    }
}

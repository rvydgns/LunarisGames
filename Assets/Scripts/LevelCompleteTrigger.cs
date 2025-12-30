using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public int thisLevelNumber; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UnlockNextLevel();
            SceneManager.LoadScene("MapScene");
        }
    }

    void UnlockNextLevel()
    {
        int unlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (thisLevelNumber >= unlocked)
        {
            PlayerPrefs.SetInt("UnlockedLevel", thisLevelNumber + 1);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgressManager : MonoBehaviour
{
    public static LevelProgressManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CompleteLevel(int levelIndex)
    {
        // Level tamamlandı kaydı
        PlayerPrefs.SetInt("Level_" + levelIndex, 1);

        Debug.Log("Level " + levelIndex + " completed!");

        // Map ekranına dön
        SceneManager.LoadScene("MapScene");
    }

    public bool IsLevelCompleted(int levelIndex)
    {
        return PlayerPrefs.GetInt("Level_" + levelIndex, 0) == 1;
    }
}

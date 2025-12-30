using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndToCutscene : MonoBehaviour
{
    [SerializeField] private string endCutsceneSceneName = "EndCutscene";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 1f; 
            SceneManager.LoadScene(endCutsceneSceneName);
        }
    }
}
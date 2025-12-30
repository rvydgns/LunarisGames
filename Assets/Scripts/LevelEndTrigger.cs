using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndTrigger : MonoBehaviour
{
    public int levelIndex; // bu levelin numarası (1,2,3...)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 🔓 Bir sonraki leveli aç
            MapManager.UnlockNextLevel(levelIndex);

            // 🗺 Map ekranına dön
            SceneManager.LoadScene("MapScene");
        }
    }
}
